(function () {
    $.fn.form = function () {
        var Form = function ($form) {
            this.$form = $form;
            this.$form.on('click', '[data-action-click]', function (e) {

                var $action = $(this);
                var command = $action.data("action-click");
                var parameter = $action.data("action-parameter");
                $form.trigger(command, {
                    context: $action,
                    parameter: parameter
                });
                return false;
            });
        }
        Form.prototype = {
            getInputs: function () {
                var inputs = {};
                this.$form.find('.form-control').each(function () {
                    var $input = $(this);
                    inputs[$input.attr("name")] = $input;
                });
                return inputs;
            },
            getInputValue: function (name) {
                var inputs = this.getInputs();
                return inputs[name].val();
            },
            setInputValue: function (name, value) {
                var inputs = this.getInputs();
                return inputs[name].val(value);
            },
            clear: function () {
                var inputs = this.getInputs();
                for (var name in inputs) {
                    inputs[name].val("");
                }
            },
            on: function (event, callback) {
                var self = this;
                self.$form.on(event, function (e, params) {
                    callback.call(self, e, params);
                });
            },
            trigger: function (event, params) {
                var self = this;
                self.$form.trigger(event, params);
            },
            hideError: function () {
                var self = this;
                self.$form.hideError();
            },
            showError: function (errors) {
                var self = this;
                self.$form.showError(errors);
            },
            handleAJAXError: function (err) {
                var self = this;
                if (err.responseJSON.ModelState) {
                    var errors = [];
                    for (var prop in err.responseJSON.ModelState) {
                        errors.push(err.responseJSON.ModelState[prop][0]);
                    }
                    self.showError(errors);
                } else {
                    self.showError([JSON.parse(err.responseText).ExceptionMessage]);
                }
            }
        };
        var forms = [];
        $(this).each(function () {
            forms.push(new Form($(this)));
        });
        return forms;
    };
})();