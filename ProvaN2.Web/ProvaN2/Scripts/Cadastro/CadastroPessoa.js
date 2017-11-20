function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#cpf').val(dados.CPF);
    $('#idade').val(dados.Idade);
    $('#txt_email').val(dados.Email);
    $('#sexo').val(dados.Sexo);
}

function set_focus_form() {
    $('#txt_nome').focus();
}

function abrir_form(dados) {
    set_dados_form(dados);

    var modal_cadastro = $('#modal_cadastro');

    bootbox.dialog({
        title: 'Cadastro de Pessoas',
        message: modal_cadastro
    })
        .on('shown.bs.modal', function () {
            modal_cadastro.show(0, function () {
                set_focus_form();
            });
        })
        .on('hidden.bs.modal', function () {
            modal_cadastro.hide().appendTo('body');
        });
}

function animacao_load() {
    bootbox.dialog({
        title: 'Carregando',
        message: '<p><i class="fa fa-spin fa-spinner"></i> Loading...</p>'
    })
}

function criar_linha_grid(dados) {
    var ret =
        '<tr data-id=' + dados.Id + '>' +
        '<td class="col-md-3">' + dados.Nome + '</td>' +
        '<td class="col-md-3">' + dados.CPF + '</td>' +
        '<td class="col-md-2">' + dados.Email + '</td>' +
        '<td>' + dados.Idade + '</td>' +
        '<td>' + dados.Sexo + '</td>' +
        '<td>' +
        '<a class="btn btn-primary btn-alterar" role="button" style="margin-right: 3px"><i class="glyphicon glyphicon-pencil"></i> Alterar</a>' +
        '<a class="btn btn-danger btn-excluir" role="button"><i class="glyphicon glyphicon-trash"></i> Excluir</a>' +
        '</td>' +
        '</tr>';

    return ret;
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.CPF).end()
        .eq(2).html(param.Email).end()
        .eq(3).html(param.Idade).end()
        .eq(4).html(param.Sexo).end()        
}

$(document).on('click', '#btn_incluir', function () {
    abrir_form({ Id: 0, CPF: ' ', Nome: ' ', Idade: 0, Sexo: ' ', Email: ' ' });
})
    .on('click', '.btn-excluir', function () {
        var btn = $(this),
            tr = btn.closest('tr'),
            id = tr.attr('data-id'),
            url = url_excluir,
            param = { 'id': id };

        bootbox.confirm({
            message: "Realmente deseja excluir a pessoa?",
            buttons: {
                confirm: {
                    label: 'Sim',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'Não',
                    className: 'btn-success'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post(url, param, function (response) {
                        if (response) {
                            tr.remove();
                            var quant = $('#grid_cadastro > tbody > tr').length;
                            if (quant == 0) {
                                $('#mensagem_grid').toggleClass('invisivel');
                                $('#grid_cadastro').toggleClass('invisivel');
                            }
                        }
                    });
                }
            }
        });
    })

    .on('click', '.btn-alterar', function () {
        var btn = $(this),
            id = btn.closest('tr').attr('data-id'),
            url = url_alterar,
            param = { 'id': id };

        $.post(url, param, function (response) {
            if (response) {
                abrir_form(response);
            }
        });
    })
    .on('click', '#btn_confirmar', function () {
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
            if (response) {
                if (param.Id == 0) {
                    param.Id = response.IdSalvo;
                    var table = $('#grid_cadastro').find('tbody'),
                        linha = criar_linha_grid(param);

                    table.append(linha)
                }
                else {
                    var linha = $('#grid_cadastro').find('tr[data-id=' + param.Id + ']').find('td');
                    preencher_linha_grid(param, linha);
                }
            }
            $('#modal_cadastro').parents('.bootbox').modal('hide');
        });
    })

    .on('click', '#btn_buscar', function () {
        url = url_buscar;
        var Busca = $('#tx_busca').val();
        param = { 'Busca': Busca };

        $.post(url, param, function (response) {
            if (response) {

            }
        })
    });    