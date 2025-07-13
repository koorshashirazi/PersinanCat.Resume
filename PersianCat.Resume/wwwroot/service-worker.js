const CACHE_NAME = 'blazor-cache-v1';
const urlsToCache = [
    './',
    './css/main.css',
    './css/bootstrap/bootstrap.min.css',
    './css/aos.css',
    './css/roadmap.css',
    './css/blazor.css',
    './js/main.js',
    './js/aos.js',
    './js/BigPicture.min.js',
    './js/bootstrap/bootstrap.bundle.min.js',
    './js/experience-animation.js',
    './js/imagesloaded.pkgd.js',
    './js/masonry.pkgd.min.js',
    './js/purecounter.min.js',
    './js/skills-animation.js',
    '/_framework/blazor.webassembly.js',
    '/_framework/blazor.boot.json',
    '/_framework/dotnet.wasm',
    '/_framework/dotnet.js',
    './Resources/en-US.json',
    './Resources/fa-IR.json',
    './Resources/de-DE.json',
    './images/**/*.png',
    './images/**/*.jpg',
    './images/**/*.jpeg',
    './images/**/*.svg',
    './images/**/*.webp',
    './images/favicon.ico',
];

self.addEventListener('install', event => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => cache.addAll(urlsToCache))
    );
});

self.addEventListener('fetch', event => {
    event.respondWith(
        caches.match(event.request)
            .then(response => response || fetch(event.request))
    );
});