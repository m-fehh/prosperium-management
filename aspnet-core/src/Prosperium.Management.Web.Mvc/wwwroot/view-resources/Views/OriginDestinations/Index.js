(function ($) {
    var _categoriesService = abp.services.app.category,
        l = abp.localization.getSource('Management'),
        _$modal = $('#CategoryEditModal'),
        _$table = $('#CategoriesTable');


    console.log("_categoriesService", _categoriesService);
    console.log("_$table", _$table);

    var _$categoriesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _categoriesService.getAllCategoriesPluggy,
            inputFilter: function () {
                return $('#CategorySearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$categoriesTable.draw(false)
            }
        ],
        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
            },
            {
                targets: 1,
                data: 'category.pluggy_Category_Name_Translated',
                sortable: false
            },
            {
                targets: 2,
                data: 'category.pluggy_Description_Translated',
                sortable: false,
            },
            {
                targets: 3,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    console.log("piru", row);

                    return [
                        `<button type="button" style="background: orange; border: 0; color: #fff;" class="btn btn-sm edit-category" data-category-pluggy-id="${row.category.id}" data-toggle="modal" data-target="#CategoryEditModal">
                            <i class="fas fa-pencil-alt"></i>
                        </button>`
                    ].join('');
                }
            }
        ]
    });


    $(document).on('click', '.edit-category', function (e) {
        var pluggyId = $(this).data('category-pluggy-id');

        console.log(pluggyId);

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'App/OriginDestinations/GetDestinations',
            type: 'GET',
            data: { pluggyId: pluggyId },
            dataType: 'html',
            success: function (content) {
                $('#CategoryEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', '#saveOriginDestination', function (e) {
        e.preventDefault();

        var pluggyId = $("#pluggyId").val();
        var selectedCategoryId = $('#categorySelect').val();

        console.log("pluggyId", +pluggyId);
        console.log("selectedCategoryId", +selectedCategoryId);

        _categoriesService.updateCategoryPluggy(+pluggyId, +selectedCategoryId).done(function () {
            _$modal.modal('hide');
            abp.notify.info(l('SavedSuccessfully'));
            _$categoriesTable.ajax.reload();
        }).always(function () {
            abp.ui.clearBusy(_$modal);
        });
    });
})(jQuery);
