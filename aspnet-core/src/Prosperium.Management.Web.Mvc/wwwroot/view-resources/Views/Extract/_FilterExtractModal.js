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

        var selectedCategoryIds = $('.row.category-row .card.selected').map(function () {
            return $(this).data('categoria-id');
        }).get();

        var selectedTypeIds = $('.row.transaction-type-row .card.selected').map(function () {
            return $(this).data('tipo-id');
        }).get();

        console.log(selectedAccountIds);
        console.log(selectedCategoryIds);
        console.log(selectedTypeIds);

        $('#selectedAccount').val(selectedAccountIds.join(','));
        $('#selectedCategory').val(selectedCategoryIds.join(','));
        $('#selectedType').val(selectedTypeIds.join(','));

        filter();
    });

    _$modal.on('shown.bs.modal', function () {
        _$form.find('input[type=text]:first').focus();
    });



})(jQuery);