// Generated by CoffeeScript 1.12.2
(function() {
  var ComposedVarying, FlatMappedVarying, FlattenedVarying, ManagedVarying, MappedVarying, Observation, Varying, fix, identity, isFunction, nothing, ref, uniqueId,
    slice = [].slice,
    extend = function(child, parent) { for (var key in parent) { if (hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; },
    hasProp = {}.hasOwnProperty;

  ref = require('../util/util'), isFunction = ref.isFunction, fix = ref.fix, uniqueId = ref.uniqueId;

  Varying = (function() {
    var _pure;

    Varying.prototype.isVarying = true;

    function Varying(value) {
      this.set(value);
      this._observers = {};
      this._refCount = 0;
      this._generation = 0;
    }

    Varying.prototype.map = function(f) {
      return new MappedVarying(this, f);
    };

    Varying.prototype.flatten = function() {
      return new FlattenedVarying(this);
    };

    Varying.prototype.flatMap = function(f) {
      return new FlatMappedVarying(this, f);
    };

    Varying.prototype.reactLater = function(f_) {
      var id, ref1;
      id = uniqueId();
      this._refCount += 1;
      if ((ref1 = this.refCount$) != null) {
        ref1.set(this._refCount);
      }
      return this._observers[id] = new Observation(this, id, f_, (function(_this) {
        return function() {
          var ref2;
          delete _this._observers[id];
          _this._refCount -= 1;
          return (ref2 = _this.refCount$) != null ? ref2.set(_this._refCount) : void 0;
        };
      })(this));
    };

    Varying.prototype.react = function(f_) {
      var observation;
      observation = this.reactLater(f_);
      f_.call(observation, this.get());
      return observation;
    };

    Varying.prototype.set = function(value) {
      var _, generation, observer, ref1;
      if (value === this._value) {
        return;
      }
      generation = this._generation += 1;
      this._value = value;
      ref1 = this._observers;
      for (_ in ref1) {
        observer = ref1[_];
        observer.f_(this._value);
        if (generation !== this._generation) {
          return;
        }
      }
      return null;
    };

    Varying.prototype.get = function() {
      return this._value;
    };

    Varying.prototype.pipe = function(f) {
      return f(this);
    };

    Varying.prototype.refCount = function() {
      return this.refCount$ != null ? this.refCount$ : this.refCount$ = new Varying(this._refCount);
    };

    Varying.prototype.bind = function(other) {
      var k, ref1, v;
      ref1 = FlatMappedVarying.prototype;
      for (k in ref1) {
        v = ref1[k];
        this[k] = v;
      }
      return FlatMappedVarying.call(this, other);
    };

    _pure = function(flat) {
      return function() {
        var args, f;
        args = 1 <= arguments.length ? slice.call(arguments, 0) : [];
        if (isFunction(args[0]) && (args[0].react == null)) {
          f = args[0];
          return (fix(function(curry) {
            return function(args) {
              if (args.length < f.length) {
                return function() {
                  var more;
                  more = 1 <= arguments.length ? slice.call(arguments, 0) : [];
                  return curry(args.concat(more));
                };
              } else {
                return new ComposedVarying(args, f, flat);
              }
            };
          }))(args.slice(1));
        } else {
          f = args.pop();
          return new ComposedVarying(args, f, flat);
        }
      };
    };

    Varying.pure = _pure(false);

    Varying.mapAll = Varying.pure;

    Varying.flatMapAll = _pure(true);

    Varying.lift = function(f) {
      return function() {
        var args;
        args = 1 <= arguments.length ? slice.call(arguments, 0) : [];
        return new ComposedVarying(args, f, false);
      };
    };

    Varying.managed = function() {
      var computation, i, resources;
      resources = 2 <= arguments.length ? slice.call(arguments, 0, i = arguments.length - 1) : (i = 0, []), computation = arguments[i++];
      return new ManagedVarying(resources, computation);
    };

    Varying.ly = function(x) {
      if ((x != null ? x.isVarying : void 0) === true) {
        return x;
      } else {
        return new Varying(x);
      }
    };

    return Varying;

  })();

  Observation = (function() {
    function Observation(parent1, id1, f_1, _stop) {
      this.parent = parent1;
      this.id = id1;
      this.f_ = f_1;
      this._stop = _stop;
    }

    Observation.prototype.stop = function() {
      this.stopped = true;
      return this._stop();
    };

    return Observation;

  })();

  identity = function(x) {
    return x;
  };

  nothing = {
    isNothing: true
  };

  FlatMappedVarying = (function(superClass) {
    extend(FlatMappedVarying, superClass);

    function FlatMappedVarying(_parent, _f, _flatten) {
      this._parent = _parent;
      this._f = _f != null ? _f : identity;
      this._flatten = _flatten != null ? _flatten : true;
      this._observers = {};
      this._refCount = 0;
      this._value = nothing;
    }

    FlatMappedVarying.prototype.react = function(f_) {
      return this._react(f_, true);
    };

    FlatMappedVarying.prototype.reactLater = function(f_) {
      return this._react(f_, false);
    };

    FlatMappedVarying.prototype._react = function(callback, immediate) {
      var id, initialGeneration, observation, ref1;
      id = uniqueId();
      this._observers[id] = observation = new Observation(this, id, callback, (function(_this) {
        return function() {
          var ref1, ref2;
          delete _this._observers[id];
          _this._refCount -= 1;
          if (_this._refCount === 0) {
            if ((ref1 = _this._lastInnerObservation) != null) {
              ref1.stop();
            }
            _this._parentObservation.stop();
            _this._value = nothing;
          }
          return (ref2 = _this.refCount$) != null ? ref2.set(_this._refCount) : void 0;
        };
      })(this));
      if (this._refCount === 0) {
        this._lastInnerObservation = null;
        this._generation = 0;
        this._parentObservation = this._bind();
      }
      initialGeneration = this._generation;
      this._refCount += 1;
      if ((ref1 = this.refCount$) != null) {
        ref1.set(this._refCount);
      }
      if ((this._generation === initialGeneration) && (this._flatten === true || immediate === true)) {
        if (this._value === nothing) {
          this._onValue(this._parentObservation, this._immediate(), !immediate);
        } else if (immediate === true) {
          callback.call(observation, this._value);
        }
      }
      return observation;
    };

    FlatMappedVarying.prototype._onValue = function(observation, value, silent) {
      var _, generation, o, ref1, ref2, self;
      if (silent == null) {
        silent = false;
      }
      self = this;
      if (this._flatten === true && observation === this._parentObservation) {
        if ((ref1 = this._lastInnerObservation) != null) {
          ref1.stop();
        }
        if ((value != null ? value.isVarying : void 0) === true) {
          this._lastInnerObservation = value.react(function(raw) {
            return self._onValue(this, raw, silent);
          });
          silent = false;
          return;
        } else {
          this._lastInnerObservation = null;
        }
      }
      if ((value !== this._value) && (silent !== true)) {
        generation = (this._generation += 1);
        ref2 = this._observers;
        for (_ in ref2) {
          o = ref2[_];
          if (generation === this._generation) {
            o.f_(value);
          }
        }
      }
      this._value = value;
      silent = false;
      return null;
    };

    FlatMappedVarying.prototype._bind = function() {
      return this._parent.reactLater((function(_this) {
        return function(raw) {
          return _this._onValue(_this._parentObservation, _this._f.call(null, raw));
        };
      })(this));
    };

    FlatMappedVarying.prototype._immediate = function() {
      if (this._value === nothing) {
        return this._f.call(null, this._parent.get());
      } else {
        return this._value;
      }
    };

    FlatMappedVarying.prototype.set = void 0;

    FlatMappedVarying.prototype.bind = void 0;

    FlatMappedVarying.prototype.get = function() {
      var result;
      result = this._immediate();
      if (this._flatten === true && (result != null ? result.isVarying : void 0) === true) {
        return result.get();
      } else {
        return result;
      }
    };

    return FlatMappedVarying;

  })(Varying);

  FlattenedVarying = (function(superClass) {
    extend(FlattenedVarying, superClass);

    function FlattenedVarying(parent) {
      FlattenedVarying.__super__.constructor.call(this, parent);
    }

    return FlattenedVarying;

  })(FlatMappedVarying);

  MappedVarying = (function(superClass) {
    extend(MappedVarying, superClass);

    function MappedVarying(parent, f) {
      MappedVarying.__super__.constructor.call(this, parent, f, false);
    }

    return MappedVarying;

  })(FlatMappedVarying);

  ComposedVarying = (function(superClass) {
    extend(ComposedVarying, superClass);

    function ComposedVarying(_applicants, _f, _flatten) {
      this._applicants = _applicants;
      this._f = _f != null ? _f : identity;
      this._flatten = _flatten != null ? _flatten : false;
      this._observers = {};
      this._refCount = 0;
      this._value = nothing;
      this._allBound = false;
      this._partial = [];
      this._parentObservations = [];
    }

    ComposedVarying.prototype._bind = function() {
      var a, idx;
      this._parentObservations = (function() {
        var i, len, ref1, results;
        ref1 = this._applicants;
        results = [];
        for (idx = i = 0, len = ref1.length; i < len; idx = ++i) {
          a = ref1[idx];
          results.push((function(_this) {
            return function(a, idx) {
              return a.react(function(value) {
                _this._partial[idx] = value;
                if (_this._allBound === true) {
                  _this._onValue(_this._parentObservation, _this._f.apply(_this._parentObservations[idx], _this._partial));
                }
                return null;
              });
            };
          })(this)(a, idx));
        }
        return results;
      }).call(this);
      this._allBound = true;
      return new Observation(this, uniqueId(), null, (function(_this) {
        return function() {
          var i, len, ref1, results, v;
          ref1 = _this._parentObservations;
          results = [];
          for (i = 0, len = ref1.length; i < len; i++) {
            v = ref1[i];
            results.push(v.stop());
          }
          return results;
        };
      })(this));
    };

    ComposedVarying.prototype._immediate = function() {
      var a;
      if (this._value === nothing) {
        if (this._allBound === true) {
          return this._f.apply(null, this._partial);
        } else {
          return this._f.apply(null, (function() {
            var i, len, ref1, results;
            ref1 = this._applicants;
            results = [];
            for (i = 0, len = ref1.length; i < len; i++) {
              a = ref1[i];
              results.push(a.get());
            }
            return results;
          }).call(this));
        }
      } else {
        return this._value;
      }
    };

    return ComposedVarying;

  })(FlatMappedVarying);

  ManagedVarying = (function(superClass) {
    extend(ManagedVarying, superClass);

    function ManagedVarying(_resources, _computation) {
      var resources;
      this._resources = _resources;
      this._computation = _computation;
      ManagedVarying.__super__.constructor.call(this, new Varying());
      this._awake = false;
      resources = null;
      this.refCount().reactLater((function(_this) {
        return function(count) {
          var f, i, len, resource, results;
          if (count > 0 && _this._awake === false) {
            _this._awake = true;
            resources = (function() {
              var i, len, ref1, results;
              ref1 = this._resources;
              results = [];
              for (i = 0, len = ref1.length; i < len; i++) {
                f = ref1[i];
                results.push(f());
              }
              return results;
            }).call(_this);
            return _this._parent.set(_this._computation.apply(null, resources));
          } else if (count === 0 && _this._awake === true) {
            _this._awake = false;
            results = [];
            for (i = 0, len = resources.length; i < len; i++) {
              resource = resources[i];
              results.push(resource.destroy());
            }
            return results;
          }
        };
      })(this));
    }

    ManagedVarying.prototype.get = function() {
      var result;
      if (this._awake === true) {
        return ManagedVarying.__super__.get.call(this);
      } else {
        result = null;
        this.react(function(x) {
          result = x;
          return this.stop();
        });
        return result;
      }
    };

    return ManagedVarying;

  })(FlatMappedVarying);

  module.exports = {
    Varying: Varying,
    Observation: Observation,
    FlatMappedVarying: FlatMappedVarying,
    FlattenedVarying: FlattenedVarying,
    MappedVarying: MappedVarying,
    ComposedVarying: ComposedVarying
  };

}).call(this);