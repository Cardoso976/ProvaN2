function abrir_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#cpf').val(dados.CPF);
    $('#idade').val(dados.Idade);
    $('#txt_email').val(dados.Email);
    $('#sexo').val(dados.Sexo);

    var modal_cadastro = $("#modal_cadastro");

    bootbox.dialog({
        title: 'Cadastro de Pessoas',
        message: modal_cadastro
    })
    .on('shown.bs.modal', function () {
        modal_cadastro.show(0, function () {
            $("#txt_nome").focus();
        });
    });
}

function animacao_load(){
    bootbox.dialog({
        title: 'Carregando',
        message: '<p><i class="fa fa-spin fa-spinner"></i> Loading...</p>'
    })    
}

function criar_linha_grid(dados) {
    var ret =
        '<tr data-id=' + dados.Id + '>' +
        '<td class="col-md-3">' + dados.Nome + '</td>' +
        '<td>' + dados.CPF + '</td>' +
        '<td class="col-md-2">' + dados.Idade + '</td>' +
        '<td>' + dados.Sexo + '</td>' +
        '<td>' + dados.Email + '</td>' +
        '<td>' +
        '<a class="btn btn-primary btn-alterar" role="button" style="margin-right: 3px"><i class="glyphicon glyphicon-pencil"></i> Alterar</a>' +
        '<a class="btn btn-danger btn-excluir" role="button"><i class="glyphicon glyphicon-trash"></i> Excluir</a>' +
        '</td>' +
        '</tr>';

    return ret;
}



$("#btn_incluir").click(function () {
    abrir_form({
        Id: 0,
        CPF: '',
        Nome: '',
        Idade: 0,
        Sexo: '',
        Email: ''
    });    
});

$('#btn_confirmar').click(function (){
    var btn = $(this),
        param = {
            Id: $('#id_cadastro').val(),            
            CPF: $('#cpf').val(),
            Nome: $('#txt_nome').val(),           
            Idade: $('#idade').val(),           
            Sexo: $('#sexo').val(),
            Email: $('#txt_email').val()
        };
    $.post(url_confirmar, param, function (response) {
        if(response){
            if (param.Id == 0) {
                param.Id = response.IdSalvo;
                var table = $('#grid_cadastro').find('tbody'),
                    linha = criar_linha_grid(param);

                table.append(linha)
            }
        }
        $('#modal_cadastro').parents('.bootbox').modal('hide');
    });
});