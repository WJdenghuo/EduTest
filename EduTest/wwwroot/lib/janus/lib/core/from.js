// Generated by CoffeeScript 1.12.2
(function() {
  var Varying, applyMaps, build, conj, dc, defaultCases, defcase, from, ic, internalCases, mapVaryingOrRewrap, mappedPoint, match, matchFinal, otherwise, plainMap, ref, terminus, val,
    slice = [].slice;

  Varying = require('./varying').Varying;

  ref = require('./case'), defcase = ref.defcase, match = ref.match, otherwise = ref.otherwise;

  conj = function(x, y) {
    return x.concat([y]);
  };

  internalCases = ic = defcase({
    'org.janusjs.core.from.internal': {
      arity: 2
    }
  }, 'varying', 'map', 'flatMap', 'pipe', 'resolve', 'unflat');

  defaultCases = dc = defcase('org.janusjs.core.from.default', 'dynamic', 'watch', 'resolve', 'attribute', 'varying', 'app', 'self');

  val = function(conjunction, applicants) {
    var result;
    if (applicants == null) {
      applicants = [];
    }
    result = {};
    result.map = function(f) {
      var i, last, rest;
      rest = 2 <= applicants.length ? slice.call(applicants, 0, i = applicants.length - 1) : (i = 0, []), last = applicants[i++];
      return val(conjunction, conj(rest, internalCases.map(last, f)));
    };
    result.flatMap = function(f) {
      var i, last, rest;
      rest = 2 <= applicants.length ? slice.call(applicants, 0, i = applicants.length - 1) : (i = 0, []), last = applicants[i++];
      return val(conjunction, conj(rest, internalCases.flatMap(last, f)));
    };
    result.watch = function(attr, orElse) {
      var f, i, last, rest;
      if (orElse == null) {
        orElse = null;
      }
      rest = 2 <= applicants.length ? slice.call(applicants, 0, i = applicants.length - 1) : (i = 0, []), last = applicants[i++];
      f = function(obj) {
        var ref1;
        return (ref1 = obj != null ? typeof obj.watch === "function" ? obj.watch(attr) : void 0 : void 0) != null ? ref1 : orElse;
      };
      return val(conjunction, conj(rest, internalCases.flatMap(last, f)));
    };
    result.resolve = function(attr) {
      var i, last, rest;
      rest = 2 <= applicants.length ? slice.call(applicants, 0, i = applicants.length - 1) : (i = 0, []), last = applicants[i++];
      return val(conjunction, conj(rest, internalCases.resolve(last, attr)));
    };
    result.attribute = function(attr) {
      var f, i, last, rest;
      rest = 2 <= applicants.length ? slice.call(applicants, 0, i = applicants.length - 1) : (i = 0, []), last = applicants[i++];
      f = function(obj) {
        return obj != null ? typeof obj.attribute === "function" ? obj.attribute(attr) : void 0 : void 0;
      };
      return val(conjunction, conj(rest, internalCases.map(last, f)));
    };
    result.pipe = function(f) {
      var i, last, rest;
      rest = 2 <= applicants.length ? slice.call(applicants, 0, i = applicants.length - 1) : (i = 0, []), last = applicants[i++];
      return val(conjunction, conj(rest, internalCases.pipe(last, f)));
    };
    result.asVarying = function() {
      var i, last, rest;
      rest = 2 <= applicants.length ? slice.call(applicants, 0, i = applicants.length - 1) : (i = 0, []), last = applicants[i++];
      return val(conjunction, conj(rest, internalCases.unflat(last)));
    };
    result.all = terminus(applicants);
    result.and = conjunction(applicants);
    return result;
  };

  build = function(cases) {
    var base, conjunction, kase, methods, name;
    methods = {};
    for (name in cases) {
      kase = cases[name];
      if (name !== 'dynamic') {
        (function(name, kase) {
          return methods[name] = function(applicants) {
            return function(x) {
              return val(conjunction, conj(applicants, kase(x)));
            };
          };
        })(name, kase);
      }
    }
    base = cases.dynamic != null ? (function(applicants) {
      return function(x) {
        return val(conjunction, conj(applicants, cases.dynamic(x)));
      };
    }) : (function() {
      return {};
    });
    conjunction = function(applicants) {
      var k, result, v;
      if (applicants == null) {
        applicants = [];
      }
      result = base(applicants);
      for (k in methods) {
        v = methods[k];
        result[k] = v(applicants);
      }
      return result;
    };
    return conjunction();
  };

  mapVaryingOrRewrap = function(kase, f) {
    return kase(function(inner, arg, point) {
      var innerResult;
      innerResult = mappedPoint(inner, point);
      if (ic.varying.match(innerResult) === true) {
        return ic.varying(f(innerResult.value, arg, point));
      } else {
        return kase(inner, arg);
      }
    });
  };

  mappedPoint = match(mapVaryingOrRewrap(ic.map, function(x, f) {
    return x.map(f);
  }), mapVaryingOrRewrap(ic.flatMap, function(x, f) {
    return x.flatMap(f);
  }), mapVaryingOrRewrap(ic.pipe, function(x, f) {
    return f(x);
  }), mapVaryingOrRewrap(ic.resolve, function(x, attr, point) {
    return x.flatMap(function(obj) {
      if (obj != null) {
        return point(from["default"].app()).flatMap(function(app) {
          return obj.resolve(attr, app);
        });
      }
    });
  }), mapVaryingOrRewrap(ic.unflat, function(x) {
    return new Varying(x);
  }), ic.varying(function(x) {
    return ic.varying(x);
  }), otherwise(function(x, point) {
    var result;
    result = point(x);
    if ((result != null ? result.isVarying : void 0) === true) {
      return ic.varying(result);
    } else {
      return x;
    }
  }));

  matchFinal = match(ic.varying(function(x) {
    return x;
  }), otherwise(function(x) {
    return new Varying(x);
  }));

  applyMaps = function(applicants, maps) {
    var apply, first, i, len, m, rest, v;
    first = maps[0], rest = 2 <= maps.length ? slice.call(maps, 1) : [];
    v = applicants.length === 1 ? (first != null ? rest.unshift(first) : void 0, matchFinal(applicants[0])) : (first != null ? first : first = ic.map(function() {
      var args;
      args = 1 <= arguments.length ? slice.call(arguments, 0) : [];
      return args;
    }), match(ic.map(function(f) {
      var x;
      return Varying.mapAll.apply(null, ((function() {
        var i, len, results;
        results = [];
        for (i = 0, len = applicants.length; i < len; i++) {
          x = applicants[i];
          results.push(matchFinal(x));
        }
        return results;
      })()).concat([f]));
    }), ic.flatMap(function(f) {
      var x;
      return Varying.flatMapAll.apply(null, ((function() {
        var i, len, results;
        results = [];
        for (i = 0, len = applicants.length; i < len; i++) {
          x = applicants[i];
          results.push(matchFinal(x));
        }
        return results;
      })()).concat([f]));
    }), otherwise(function() {
      throw 1;
    }))(first));
    apply = match(ic.map(function(x) {
      return v.map(x);
    }), ic.flatMap(function(x) {
      return v.flatMap(x);
    }), otherwise(function() {
      throw 1;
    }));
    for (i = 0, len = rest.length; i < len; i++) {
      m = rest[i];
      v = apply(m);
    }
    return v;
  };

  plainMap = match(dc.dynamic(function(x) {
    return Varying.ly(x);
  }), dc.varying(function(x) {
    return Varying.ly(x);
  }), otherwise(function(x) {
    return x;
  }));

  terminus = function(applicants, maps) {
    var result;
    if (maps == null) {
      maps = [];
    }
    result = function(f) {
      return terminus(applicants, maps.concat([ic.flatMap(f)]));
    };
    result.flatMap = function(f) {
      return terminus(applicants, maps.concat([ic.flatMap(f)]));
    };
    result.map = function(f) {
      return terminus(applicants, maps.concat([ic.map(f)]));
    };
    result.point = function(f) {
      var x;
      return terminus((function() {
        var i, len, results;
        results = [];
        for (i = 0, len = applicants.length; i < len; i++) {
          x = applicants[i];
          results.push(mappedPoint(x, f));
        }
        return results;
      })(), maps);
    };
    result.react = function(f_) {
      return applyMaps(applicants, maps).react(f_);
    };
    result.reactLater = function(f_) {
      return applyMaps(applicants, maps).reactLater(f_);
    };
    result.plain = function() {
      return result.point(plainMap);
    };
    result.all = result;
    result.get = function() {
      var ref1;
      return (ref1 = matchFinal(mappedPoint(applicants[0], (function() {})))) != null ? ref1.get() : void 0;
    };
    result.isVarying = true;
    return result;
  };

  from = build(defaultCases);

  from.build = build;

  from["default"] = defaultCases;

  module.exports = from;

}).call(this);