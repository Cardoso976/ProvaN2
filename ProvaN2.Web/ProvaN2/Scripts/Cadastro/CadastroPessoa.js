function abrir_form() {
    var modal_cadastro = $("#modal_cadastro");

    bootbox.dialog({
        title: 'Cadastro de Pessoas',
        message: modal_cadastro
    })
        .on('shown.bs.modal', function () {
            modal_cadastro.show();
        });
}

$("#btn_incluir").click(function () {
    abrir_form();
});