//const CACHE_NAME = "my-cache";

//// Instalando o sw
//self.addEventListener("install", (e) => {
//    console.log("Instalando service worker");
//    e.waitUntil(
//        caches.open(CACHE_NAME).then((cache) => {
//            return cache
//                .addAll([`/`])
//                .then(() => self.skipWaiting());
//        })
//    );
//});

//// Ativando o sw
//self.addEventListener("activate", (event) => {
//    console.log("Ativando service worker");
//    event.waitUntil(self.clients.claim());
//});

//// Buscando as requisições e colocando em cache
//self.addEventListener("fetch", function (event) {
//    console.log(`Buscando ${event.request.url}`);

//    if (navigator.onLine) {
//        var fetchRequest = event.request.clone();
//        return fetch(fetchRequest).then(function (response) {
//            if (
//                !response ||
//                response.status !== 200 ||
//                response.type !== "basic"
//            ) {
//                return response;
//            }

//            var responseToCache = response.clone();

//            caches.open(CACHE_NAME).then(function (cache) {
//                cache.put(event.request, responseToCache);
//            });

//            return response;
//        });
//    } else {
//        event.respondWith(
//            caches.match(event.request).then(function (response) {
//                if (response) {
//                    return response;
//                }
//            })
//        );
//    }
//});