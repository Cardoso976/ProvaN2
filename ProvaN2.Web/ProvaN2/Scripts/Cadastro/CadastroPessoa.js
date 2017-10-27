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

function animacao_load(){
    bootbox.dialog({
        title: 'Carregando',
        message: '<p><i class="fa fa-spin fa-spinner"></i> Loading...</p>'
    })    
}

$("#btn_incluir").click(function () {
    abrir_form();    
});