$(document).ready(function () {
    InicializarCamera();
    FecharModalConfirmacaoCheckin();
});

function FecharModalConfirmacaoCheckin() {
    $("#botao-mensagem-sucesso").click(function () {
        $("#mensagem-sucesso").hide();
    });

    $("#botao-mensagem-erro").click(function () {
        $("#mensagem-erro").hide();
    });
}   

function InicializarCamera() {
    const camera = document.getElementById('qr-video');
    let scanner = new Instascan.Scanner({ video: camera, mirror: false, refractoryPeriod: 7000 });
    scanner.addListener('scan', function (content) {
        EfetuarCheckin(content);
    });
    Instascan.Camera.getCameras().then(function (cameras) {
        if (cameras.length > 0) {
            var backCam = cameras.find(camera => camera.name.toLowerCase().includes('back'));

            if (backCam) {

                scanner.start(backCam);
            }
            else {
                scanner.start(cameras[0]);
            }
        } else {
            console.error('No cameras found.');
        }
    }).catch(function (e) {
        console.error(e);
    });
}

function EfetuarCheckin(conteudo) {
    $.ajax({
        url: "EfetuarCheckin",
        type: 'POST',
        data: JSON.stringify({ "guid": conteudo, "TipoCheckin": 1 }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            $("#mensagem-sucesso").show();
            $("#botao-mensagem-sucesso").focus();
            $("#detalhes-sucesso").text(data.Mensagem);

            var data = new Date();
            var hora = data.getHours();
            var minuto = data.getMinutes() < 10 ? "0" + data.getMinutes() : data.getMinutes();
            $("#hora-checkin").text(hora + ":" + minuto);
        },
        error: function (data) {
            $("#mensagem-sucesso").hide();
            $("#mensagem-erro").show();
            $("#botao-mensagem-erro").focus();
            $("#detalhes-erro").html(data.responseJSON.Mensagem);
        }
    });
}

