$(document).ready((function () {
    var placeholderElement = $('#modal-placeholder');

    $(document).on('click', 'button[data-toggle="ajax-modal"]', function (event) {
        placeholderElement.find('.modal').modal('hide');
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });


    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            if (newBody.find('[name = "IsValid"]').val() == 'True') {
                tableElement = $('#elements');
                var tableUrl = tableElement.data('url');
                $.get(tableUrl).done(function (table) {
                    tableElement.replaceWith(table);
                });

                placeholderElement.find('.modal').modal('hide');
            }
            
        });
    });


    placeholderElement.on('click',
        '[data-delete="modal"]',
        function(event) {
            event.preventDefault();

            var form = $(this).parents('.modal').find('form');
            var actionUrl = form.attr('action');
            var dataToSend = form.serialize();

            $.post(actionUrl, dataToSend).done(function(data) {

                tableElement = $('#elements');
                var tableUrl = tableElement.data('url');
                $.get(tableUrl).done(function(table) {
                    tableElement.replaceWith(table);
                });

                placeholderElement.find('.modal').modal('hide');
            });
        });


}));

function gatherData() {
    var city1 = document.querySelector('input[name="city1"]:checked').value;
    var city2 = document.querySelector('input[name="city2"]:checked').value;

    var criteria1 = document.getElementById("")
}