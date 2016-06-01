$(document).ready(function () {

    var storage = window.localStorage;
    if (storage.email) {
        $('input[name="email"]').val(storage.email);
    }
    if (storage.kindle_address) {
        $('select[name="kindle_address"]').val(storage.kindle_address);
    }
    if (!storage.email && !storage.kindle_address) {
        $('#about').removeClass('hidden');
    }

    $('#post').submit(function (event) {
        event.preventDefault();
        storage.email = $('input[name="email"]:first').val();
        storage.kindle_address = $('select[name="kindle_address"]:first').val();
        $(':submit').button('loading');
        $.ajax({
                url: '/thread',
                data: {
                    submission: $('input[name="submission"]').val(),
                    comments: $('input[type="checkbox"]:first').prop('checked'),
                    email: $('input[name="email"]:first').val(),
                    kindle_address: $('select[name="kindle_address"]:first').val(),
                    comments_style: $('select[name="comments_style"]:first').val()
                },
                type: 'POST',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('X-CSRFToken', $('input[name=csrf_token]:first').attr('value'))
                }
            })
            .done(function (data) {
                $(':submit').button('reset');
                $('.modal-text').text(data.text);
                $('.modal-body').removeClass().addClass('modal-body alert alert-' + data.type);
                $('#message').modal('show');
                if (data.type == 'success') {
                    $('#post')[0].reset();
                    $('input[name="email"]').val(storage.email);
                    $('select[name="kindle_address"]').val(storage.kindle_address);
                }
            });
    });

    $('#subreddit').submit(function (event) {
        event.preventDefault();
        storage.email = $('input[name="email"]:last').val();
        storage.kindle_address = $('select[name="kindle_address"]:last').val();
        $(':submit').button('loading');
        $.ajax({
                url: '/subreddit',
                data: {
                    subreddit: $('input[name="subreddit"]').val(),
                    time: $('select[name="time"]').val(),
                    limit: $('input[name="limit"]').val(),
                    email: $('input[name="email"]:last').val(),
                    kindle_address: $('select[name="kindle_address"]:last').val()
                },
                type: 'POST',
                beforeSend: function (xhr) {
                    xhr.setRequestHeader('X-CSRFToken', $('input[name=csrf_token]:last').attr('value'))
                }
            })
            .done(function (data) {
                $(':submit').button('reset');
                $('.modal-text').text(data.text);
                $('.modal-body').removeClass().addClass('modal-body alert alert-' + data.type);
                $('#message').modal('show');
                if (data.type == 'success') {
                    $('#subreddit')[0].reset();
                    $('input[name="email"]').val(storage.email);
                    $('select[name="kindle_address"]').val(storage.kindle_address);
                }
            });
    });

    $('.collapse').on('show.bs.collapse hide.bs.collapse', function (n) {
        $(n.target).siblings('a').find('div h4 i').toggleClass('fa-chevron-up fa-chevron-down');
    });
});

