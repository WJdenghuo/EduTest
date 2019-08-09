// Generated by CoffeeScript 1.12.2
(function() {
  var Attribute, TransientAttribute, isFunction,
    extend = function(child, parent) { for (var key in parent) { if (hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; },
    hasProp = {}.hasOwnProperty,
    slice = [].slice;

  isFunction = require('../util/util').isFunction;

  Attribute = require('./attribute').Attribute;

  TransientAttribute = (function(superClass) {
    extend(_Class, superClass);

    function _Class() {
      return _Class.__super__.constructor.apply(this, arguments);
    }

    _Class.prototype.transient = true;

    return _Class;

  })(Attribute);

  module.exports = {
    attribute: function(key, klass) {
      return function(schema) {
        return schema.attributes[key] = klass;
      };
    },
    bind: function(key, binding) {
      return function(schema) {
        return schema.bindings[key] = binding;
      };
    },
    issue: function(binding) {
      return function(schema) {
        return schema.issues.push(binding);
      };
    },
    transient: function(key) {
      return function(schema) {
        return schema.attributes[key] = TransientAttribute;
      };
    },
    "default": function(key, value, klass) {
      if (klass == null) {
        klass = Attribute;
      }
      return function(schema) {
        var wrapped;
        wrapped = isFunction(value) ? value : (function() {
          return value;
        });
        return schema.attributes[key] = (function(superClass) {
          extend(_Class, superClass);

          function _Class() {
            return _Class.__super__.constructor.apply(this, arguments);
          }

          _Class.prototype["default"] = wrapped;

          return _Class;

        })(klass);
      };
    },
    Trait: function() {
      var parts;
      parts = 1 <= arguments.length ? slice.call(arguments, 0) : [];
      return function(schema) {
        var i, len, part;
        for (i = 0, len = parts.length; i < len; i++) {
          part = parts[i];
          part(schema);
        }
        return null;
      };
    }
  };

}).call(this);