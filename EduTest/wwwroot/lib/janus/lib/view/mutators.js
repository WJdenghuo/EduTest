// Generated by CoffeeScript 1.12.2
(function() {
  var Varying, doPoint, extendNew, from, isFunction, mutators, ref, safe,
    slice = [].slice;

  Varying = require('../core/varying').Varying;

  from = require('../core/from');

  ref = require('../util/util'), isFunction = ref.isFunction, extendNew = ref.extendNew;

  safe = function(x) {
    if (isFunction(x != null ? x.toString : void 0)) {
      return x.toString();
    } else {
      return '';
    }
  };

  doPoint = function(x, point) {
    if ((x != null ? x.point : void 0) != null) {
      return x.point(point);
    } else if ((x != null ? x.all : void 0) != null) {
      return x.all.point(point);
    } else {
      return Varying.ly(x);
    }
  };

  mutators = {
    attr: function(prop, data) {
      return function(dom, point) {
        return data.all.point(point).react(function(x) {
          return dom.attr(prop, safe(x));
        });
      };
    },
    classGroup: function(prefix, data) {
      return function(dom, point) {
        return data.all.point(point).react(function(x) {
          var existing, i, len, ref1, ref2, y;
          existing = (ref1 = (ref2 = dom.attr('class')) != null ? ref2.split(/[ ]+/) : void 0) != null ? ref1 : [];
          for (i = 0, len = existing.length; i < len; i++) {
            y = existing[i];
            if (y.indexOf(prefix) === 0) {
              dom.removeClass(y);
            }
          }
          return dom.addClass("" + prefix + (safe(x)));
        });
      };
    },
    classed: function(name, data) {
      return function(dom, point) {
        return data.all.point(point).react(function(x) {
          return dom.toggleClass(name, x === true);
        });
      };
    },
    css: function(prop, data) {
      return function(dom, point) {
        return data.all.point(point).react(function(x) {
          return dom.css(prop, safe(x));
        });
      };
    },
    text: function(data) {
      return function(dom, point) {
        return data.all.point(point).react(function(x) {
          return dom.text(safe(x));
        });
      };
    },
    html: function(data) {
      return function(dom, point) {
        return data.all.point(point).react(function(x) {
          return dom.html(safe(x));
        });
      };
    },
    prop: function(prop, data) {
      return function(dom, point) {
        return data.all.point(point).react(function(x) {
          return dom.prop(prop, x);
        });
      };
    },
    render: function(data, args) {
      var result;
      if (args == null) {
        args = {};
      }
      result = function(dom, point) {
        var _vendView;
        _vendView = function(subject, context, app, criteria, options) {
          return app.vendView(subject, extendNew(criteria != null ? criteria : {}, {
            context: context,
            options: options
          }));
        };
        return Varying.flatMapAll(_vendView, data.all.point(point), doPoint(args.context, point), doPoint(from.app(), point), doPoint(args.criteria, point), doPoint(args.options, point)).react(function(view) {
          var ref1;
          if (this.view == null) {
            this.view = new Varying();
          }
          if ((ref1 = this.view.get()) != null) {
            ref1.destroy();
          }
          dom.empty();
          if (view != null) {
            dom.append(view.artifact());
          }
          return this.view.set(view);
        });
      };
      result.context = function(context) {
        return mutators.render(data, extendNew(args, {
          context: context
        }));
      };
      result.criteria = function(criteria) {
        return mutators.render(data, extendNew(args, {
          criteria: criteria
        }));
      };
      result.options = function(options) {
        return mutators.render(data, extendNew(args, {
          options: options
        }));
      };
      return result;
    },
    on: function() {
      var args;
      args = 1 <= arguments.length ? slice.call(arguments, 0) : [];
      return function(dom, point) {
        return from.self().all.point(point).react(function(view) {
          var f_, g_, thisArgs;
          f_ = args[args.length - 1];
          g_ = function(event) {
            return f_(event, view.subject, view, view.artifact());
          };
          thisArgs = args.slice(0, -1);
          thisArgs.push(g_);
          return this.start = (function(_this) {
            return function() {
              dom.on.apply(dom, thisArgs);
              return _this.stop = function() {
                dom.off(thisArgs[0], g_);
                return this.constructor.prototype.stop.call(this);
              };
            };
          })(this);
        });
      };
    }
  };

  module.exports = mutators;

}).call(this);