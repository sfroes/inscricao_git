const video = document.getElementById('qr-video');

const scanner = new Instascan.Scanner({ video: video, mirror: false});

scanner.addListener('scan', function (content) {
    chamaserver(content);
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

function chamaserver(content) {
    $.ajax({

        url: 'Cam?leitura=' + content,
        type: 'GET',
        success: function (resultado) {
            var count = $("#valido li").length;
            if (resultado != null && resultado != "") {
                if (count == 0) {
                    $("#valido").append('<li><svg xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:cc="http://creativecommons.org/ns#" xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns:svg="http://www.w3.org/2000/svg" xmlns="http://www.w3.org/2000/svg" xmlns:sodipodi="http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd" xmlns:inkscape="http://www.inkscape.org/namespaces/inkscape" version="1.1" x="0px" y="0px" viewBox="0 0 100 125"><g transform="translate(0,-952.36218)"><path style="text-indent:0;text-transform:none;direction:ltr;block-progression:tb;baseline-shift:baseline;color:#000000;enable-background:accumulate;" d="m 84.885223,974.337 a 4.0004,4.0004 0 0 0 -2.75,1.21875 l -45.5,45.50005 -19.218699,-14.8438 a 4.0004,4.0004 0 1 0 -4.875,6.3438 l 21.999999,17 a 4.0004,4.0004 0 0 0 5.2813,-0.3438 l 48,-48 a 4.0004,4.0004 0 0 0 -2.9376,-6.875 z" fill="#000000" fill-opacity="1" stroke="none" marker="none" visibility="visible" display="inline" overflow="visible"/></g></svg>'
                        + resultado +
                        '</li>')
                }
                else {
                    let elementoEncontrado = false;
                    $("#valido li").each(function () {
                        if ($(this).text() === resultado) {
                            elementoEncontrado = true;
                        }
                    });

                    if (elementoEncontrado) {
                        $("#invalido").append('<li>' +
                            '<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" version="1.1" x="0px" y="0px" viewBox="0 0 100 125" enable-background="new 0 0 100 100" xml:space="preserve"><path fill="#000000" d="M74.166,26.577c-1.406-1.404-3.686-1.405-5.092,0.001L50.31,45.347L31.544,26.578  c-1.406-1.406-3.686-1.405-5.092-0.001c-1.406,1.406-1.406,3.686-0.001,5.092l18.767,18.769l-18.767,18.77  c-1.405,1.406-1.405,3.686,0.001,5.092c0.703,0.702,1.624,1.054,2.546,1.054c0.921,0,1.843-0.352,2.546-1.055L50.31,55.529  l18.765,18.769c0.703,0.703,1.625,1.055,2.546,1.055s1.843-0.352,2.546-1.054c1.406-1.406,1.406-3.686,0-5.092L55.4,50.438  l18.766-18.769C75.572,30.263,75.572,27.983,74.166,26.577z"/></svg>'
                            + resultado +
                            '</li>')
                    }
                    else {
                        $("#valido").append('<li><svg xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:cc="http://creativecommons.org/ns#" xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#" xmlns:svg="http://www.w3.org/2000/svg" xmlns="http://www.w3.org/2000/svg" xmlns:sodipodi="http://sodipodi.sourceforge.net/DTD/sodipodi-0.dtd" xmlns:inkscape="http://www.inkscape.org/namespaces/inkscape" version="1.1" x="0px" y="0px" viewBox="0 0 100 125"><g transform="translate(0,-952.36218)"><path style="text-indent:0;text-transform:none;direction:ltr;block-progression:tb;baseline-shift:baseline;color:#000000;enable-background:accumulate;" d="m 84.885223,974.337 a 4.0004,4.0004 0 0 0 -2.75,1.21875 l -45.5,45.50005 -19.218699,-14.8438 a 4.0004,4.0004 0 1 0 -4.875,6.3438 l 21.999999,17 a 4.0004,4.0004 0 0 0 5.2813,-0.3438 l 48,-48 a 4.0004,4.0004 0 0 0 -2.9376,-6.875 z" fill="#000000" fill-opacity="1" stroke="none" marker="none" visibility="visible" display="inline" overflow="visible"/></g></svg>'
                            + resultado +
                            '</li>')
                    }
                }
            }
        },
        error: function () {
            console.error('Erro na requisição AJAX.');
        }
    });
};