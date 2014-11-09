var csrftoken = $('input[name=csrf_token]').attr('value');

$.ajaxSetup({
    beforeSend: function (xhr, settings) {
        if (!/^(GET|HEAD|OPTIONS|TRACE)$/i.test(settings.type) && !this.crossDomain) {
            xhr.setRequestHeader("X-CSRFToken", csrftoken)
        }
    }
});

$(function () {
    $('#single').submit(function (event) {
        event.preventDefault();
        $(':submit').button('loading')
        $.post('/thread', {
            submission: $('input[name="submission"]').val(),
            email: $('input[name="email"]').val()
        })
            .done(function (data) {
                $(':submit').button('reset')
                $('form')
                    .before(
                        '<div class="alert alert-' + data.type + ' alert-dismissible fade in" role="alert">\
                        <button type="button" class="close" data-dismiss="alert">\
                            <span\
                            aria-hidden="true">&times;</span>\
                            <span class="sr-only">Close</span>\
                        </button>\
                        ' + data.text + '\
                    </div>'
                )
                window.setTimeout(function () {
                    $(".alert:first").alert('close');
                }, 10000);
            });
    });
});

