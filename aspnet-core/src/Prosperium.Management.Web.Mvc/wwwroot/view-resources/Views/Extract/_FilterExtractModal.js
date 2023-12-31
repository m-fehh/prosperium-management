(function ($) {
    var _$modal = $('#FilterModal'),
        _$form = _$modal.find('form');


    function filter() {
        _$modal.modal('hide');
        abp.event.trigger('filter.applied');
    }

    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();

        var selectedAccountIds = $('.row.account-row .card.selected').map(function () {
            return $(this).data('conta-id');
        }).get();

        var selectedCardIds = $('.row.creditcard-row .card.selected').map(function () {
            return $(this).data('cartao-id');
        }).get();

        var selectedCategoryIds = $('.row.category-row .card.selected').map(function () {
            return $(this).data('categoria-id');
        }).get();


        $('#selectedAccount').val(selectedAccountIds.join(','));
        $('#selectedCard').val(selectedCardIds.join(','));
        $('#selectedCategory').val(selectedCategoryIds.join(','));


        filter();
    });

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });
})(jQuery);