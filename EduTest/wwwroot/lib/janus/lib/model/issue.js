// Generated by CoffeeScript 1.12.2
(function() {
  var Base, Issue, Varying,
    extend = function(child, parent) { for (var key in parent) { if (hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; },
    hasProp = {}.hasOwnProperty;

  Base = require('../core/base').Base;

  Varying = require('../core/varying').Varying;

  Issue = (function(superClass) {
    extend(Issue, superClass);

    function Issue(arg) {
      var active, message, ref, severity, target;
      ref = arg != null ? arg : {}, active = ref.active, severity = ref.severity, message = ref.message, target = ref.target;
      this.active = Varying.ly(active != null ? active : false);
      this.severity = Varying.ly(severity != null ? severity : 0);
      this.message = Varying.ly(message != null ? message : '');
      this.target = Varying.ly(target);
    }

    return Issue;

  })(Base);

  module.exports = {
    Issue: Issue
  };

}).call(this);