
$(document).ready(function () {

    $("#CPF_Beneficiario").inputmask("mask", { "mask": "999.999.999-99" }, { reverse: true });

    $('#formModal').submit(function (e) {
        e.preventDefault();
        console.log('alteracao', alteracao, urlAltBeneficiario, urlIncBeneficiario, $(this).find("#Id_Beneficiario").val())
        $.ajax({
            url: alteracao ? urlAltBeneficiario : urlIncBeneficiario,
            method: alteracao? "PUT" : "POST",
            data: {
                "ID": alteracao ? $(this).find("#Id_Beneficiario").val() : null,
                "NOME": $(this).find("#Nome_Beneficiario").val(),
                "CPF": $(this).find("#CPF_Beneficiario").val(),
                "IDCLIENTE": idCliente
            },
            error:
                function (r) {
                    if (r.status == 400 || r.status == 420 || r.status == 422)
                        ModalDialogBeneficiario("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialogBeneficiario("Ocorreu um erro", "Ocorreu um erro interno no servidor.");                    
                },
            success:
                function (r) {
                    ModalDialogBeneficiario("Sucesso!", r)
                    $("#formModal")[0].reset();
                    RefreshTable(idCliente);
                    alteracao = false;
                }
        });
    })

    $('.select-item').click(function (e) {
        e.preventDefault();
        alteracao = true;

        console.log('wsas');

        var id = $(this).data('id');
        var nome = $(this).data('nome');
        var cpf = $(this).data('cpf');        

        $('#Id_Beneficiario').val(id);
        $('#Nome_Beneficiario').val(nome);
        $('#CPF_Beneficiario').val(cpf);
    });

    $('#btnTeste').click(function (e) {
        console.log('teste');
    });

    $('.delete-item').click(function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var idcliente = $(this).data('idcliente');

        $.ajax({
            url: urlPostDeleteBeneficiario,
            method: "DELETE",
            data: {
                "ID": id,
                "IDCLIENTE": idcliente
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialogBeneficiario("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialogBeneficiario("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (r) {
                    ModalDialogBeneficiario("Sucesso!", r)
                    $("#formModal")[0].reset();
                    RefreshTable(idcliente);
                    alteracao = false;
                }
        });
    });

})

function ModalDialogBeneficiario(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}

function RefreshTable(idCliente) {    
    $.ajax({
        url: urlGetBeneficiario,
        type: 'GET',
        data: {
            idCliente: idCliente
        },
        success: function (data) {
            console.log(data);
            $('#itemsTable').html(data);
            $('#tblBeneficiarios').DataTable({
                "destroy": true, //use for reinitialize datatable
            });
            
        },
        error: function (xhr, status, error) {
            alert('Um erro ocorreu ao atualizar a tabela: ' + error);
        }
    });
}

