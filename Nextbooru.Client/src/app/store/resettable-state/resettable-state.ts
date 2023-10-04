import { Action, State } from "@ngxs/store";
import { StateClass } from "@ngxs/store/internals";
import { StateContext, StoreOptions } from "@ngxs/store/src/symbols";
import { __decorate } from "tslib";

export class ResetState {
  static readonly type = "[RessetableState] Reset state";
}

export function ResettableState<T = any>(options: StoreOptions<T>) {
  return (target: StateClass) => {
    __decorate([State<T>(options)], target);

    target.prototype.__handleReset = function(ctx: StateContext<T>) {
      ctx.setState(options.defaults!);
    }

    __decorate([Action(ResetState)], target.prototype, '__handleReset');
  }
}
