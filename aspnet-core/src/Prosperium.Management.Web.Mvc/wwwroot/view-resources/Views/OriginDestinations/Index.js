(function ($) {
    var _originDestinationservice = abp.services.app.originDestination,
        l = abp.localization.getSource('Management'),
        _$modal = $('#DestinationEditModal'),
        _$table = $('#DestinationsTable');

    var _$DestinationsTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        listAction: {
            ajaxFunction: _originDestinationservice.getAllOriginDestination,
            inputFilter: function () {
                return $('#destinationSearchForm').serializeFormToObject(true);
            }
        },
        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$DestinationsTable.draw(false)
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
                data: 'originDestinations.originPortal',
                sortable: false
            },
            {
                targets: 2,
                data: 'originDestinations.originValueName',
                sortable: false
            },
            {
                targets: 3,
                data: 'originDestinations.originValueId',
                sortable: false
            },
            {
                targets: 4,
                data: 'originDestinations.discriminator',
                sortable: false
            },
            {
                targets: 5,
                data: 'originDestinations.destinationPortal',
                sortable: false
            },
            {
                targets: 6,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                render: (data, type, row, meta) => {
                    return [
                        `<button type="button" style="background: orange; border: 0; color: #fff;" class="btn btn-sm edit-destination" data-destination-pluggy-id="${row.originDestinations.id}" data-discriminator="${row.originDestinations.discriminator}" data-toggle="modal" data-target="#DestinationEditModal">
                            <i class="fas fa-pencil-alt"></i>
                        </button>`
                    ].join('');
                }
            }
        ]
    });


    $(document).on('click', '.edit-destination', function (e) {
        var pluggyId = $(this).data('destination-pluggy-id');
        var discriminator = $(this).data('discriminator');

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'App/originDestinations/GetDestinations',
            type: 'GET',
            data: { pluggyId: pluggyId, discriminator: discriminator },
            dataType: 'html',
            success: function (content) {
                $('#DestinationEditModal div.modal-content').html(content);
            },
            error: function (e) {
            }
        });
    });

    $(document).on('click', '#saveOriginDestination', function (e) {
        e.preventDefault();

        $("#confirmacaoAlterarTodosModal").show();

        var pluggyId = $("#pluggyId").val();
        var selecteddestinationId = $('#destinationSelect').val();

        $("#AlterarTodos").click(function () {
            _originDestinationservice.updateDestinationValue(pluggyId, selecteddestinationId, true).done(function () {
                _$modal.modal('hide');
                abp.notify.info(l('SavedSuccessfully'));
                _$DestinationsTable.ajax.reload();
            }).always(function () {
                abp.ui.clearBusy(_$modal);
            });

            $("#confirmacaoAlterarTodosModal").hide();
        });

        $("#AlterarApenasEste").click(function () {
            _originDestinationservice.updateDestinationValue(pluggyId, selecteddestinationId, false).done(function () {
                _$modal.modal('hide');
                abp.notify.info(l('SavedSuccessfully'));
                _$DestinationsTable.ajax.reload();
            }).always(function () {
                abp.ui.clearBusy(_$modal);
            });

            $("#confirmacaoAlterarTodosModal").hide();
        });
    });

})(jQuery);
