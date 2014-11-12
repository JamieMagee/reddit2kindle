$(document).ready(function () {

    $('#post').submit(function (event) {
        event.preventDefault();
        $(':submit').button('loading');
        $.ajax({
            url: '/thread',
            data: {
                submission: $('input[name="submission"]').val(),
                email: $('input[name="email"]:first').val()
            },
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('X-CSRFToken', $('input[name=csrf_token]:first').attr('value'))
            }
        })
            .done(function (data) {
                $(':submit').button('reset');
                $('.container:last')
                    .prepend(
                        '<div class="alert alert-' + data.type + ' alert-dismissible fade in" role="alert">\
                        <button type="button" class="close" data-dismiss="alert">\
                            <span\
                            aria-hidden="true">&times;</span>\
                            <span class="sr-only">Close</span>\
                        </button>\
                        ' + data.text + '\
                    </div>'
                );
                window.setTimeout(function () {
                    $(".alert:first").alert('close');
                }, 10000);
                if (data.type == 'success') {
                    $('#post')[0].reset();
                }
            });
    });

    $('#subreddit').submit(function (event) {
        event.preventDefault();
        $(':submit').button('loading');
        $.ajax({
            url: '/subreddit',
            data: {
                subreddit: $('input[name="subreddit"]').val(),
                time: $('select').val(),
                limit: $('input[name="limit"]').val(),
                email: $('input[name="email"]:last').val()
            },
            type: 'POST',
            beforeSend: function (xhr) {
                xhr.setRequestHeader('X-CSRFToken', $('input[name=csrf_token]:last').attr('value'))
            }
        })
            .done(function (data) {
                console.log(data);
                $(':submit').button('reset');
                $('.container:last')
                    .prepend(
                        '<div class="alert alert-' + data.type + ' alert-dismissible fade in" role="alert">\
                        <button type="button" class="close" data-dismiss="alert">\
                            <span\
                            aria-hidden="true">&times;</span>\
                            <span class="sr-only">Close</span>\
                        </button>\
                        ' + data.text + '\
                    </div>'
                );
                window.setTimeout(function () {
                    $(".alert:first").alert('close');
                }, 10000);
                if (data.type == 'success') {
                    $('#subreddit')[0].reset();
                }
            });
    });

    $('.collapse').on('show.bs.collapse hide.bs.collapse', function (n) {
        $(n.target).siblings('.panel-heading').find('a i').toggleClass('fa-chevron-up fa-chevron-down');
    });
})
;

