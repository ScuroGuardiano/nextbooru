import * as util from 'node:util';
import got from 'got';
import FormData from 'form-data';
import { CookieJar } from 'tough-cookie';
import cliProgress from 'cli-progress';
import colors from 'ansi-colors';

const nextbooruUploadPath = "http://localhost:1337/api/upload";
const nextbooruLoginPath = "http://localhost:1337/api/auth/login";
const nextbooruCredentials = { username: "sgtst", password: "zaq1@WSX" };
const nextbooruCookieJar = new CookieJar();
const baseUrl = "https://safebooru.org";
const postsList = "/index.php?page=dapi&s=post&q=index&json=1&limit=100&pid=";
const postPage = "/index.php?page=post&s=view&id=";
const imagePath = "/images/%s/%s";
const step = 100;
const toUpload = 10000;
// Alright, gelbooru has classic pagination so some images can be uploaded
// while we download let's say first page, after switching to second page
// we will have duplicates, so here's my way to skip duplicates.
const uploadedIds = new Set();

const progressBar = new cliProgress.SingleBar({}, cliProgress.Presets.shades_classic);

const delay = ms => new Promise(resolve => setTimeout(resolve, ms));

function getGelbooruListUrl(page) {
    return `${baseUrl}${postsList}${step * (page - 1)}`;
}

function getGelbooruImagePath(entry) {
    return util.format(`${baseUrl}${imagePath}`, entry.directory, entry.image);
}

async function loginToNextbooru(credentials) {
    console.log("Logging in to Nextbooru...");
    try {
        await got.post(nextbooruLoginPath, {
            json: {
                username: credentials.username,
                password: credentials.password
            },
            cookieJar: nextbooruCookieJar
        });
        console.log("Login succeed.");
    }
    catch(err) {
        console.log("Login failed.")
        throw err;
    }
}

async function uploadImageToNextbooru(title, source, tags, image) {
    const form = new FormData();
    form.append("file", image.stream, {
        filename: image.filename,
        contentType: image.contentType,
        knownLength: image.length
    });
    form.append("tags", tags);
    form.append("source", source);
    form.append("title", title);

    try {
        await got.post(nextbooruUploadPath, {
            cookieJar: nextbooruCookieJar,
            body: form
        })
    }
    catch(err) {
        console.log(err.response);
    }
}

let uploaded = 0;
let page = 1;

await loginToNextbooru(nextbooruCredentials);

console.log("Stealing", toUpload, "images and uploading 'em to nextbooru...");
progressBar.start(toUpload, 0, {
    "speed": "N/A"
});

while (uploaded < toUpload) {
    const list = await got(getGelbooruListUrl(page), { retry: { limit: 3 } }).json();
    
    for (const entry of list) {
        if (uploadedIds.has(entry.id)) {
            continue;
        }
        const image = await got(getGelbooruImagePath(entry));
        await uploadImageToNextbooru(
            "Uploaded from Nextbooru.GelbooruStealer",
            `${baseUrl}${postPage}${entry.id}`,
            entry.tags,
            {
                filename: entry.image,
                contentType: image.headers['content-type'],
                length: image.headers['content-length'],
                stream: image.rawBody
            }
        );
        uploadedIds.add(entry.id);
        uploaded++;
        progressBar.increment();
        await delay(3000); // I don't want to go too fast so they won't ban me
    }

    page++;
}