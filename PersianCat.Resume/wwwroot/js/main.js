window.blazorInterop = {

    initializeAOS: function () {
        AOS.init({
            anchorPlacement: 'top-left',
            duration: 600,
            easing: "ease-in-out",
            once: true,
            mirror: false,
            disable: 'mobile'
        });
    },
    initializeScrollTop: function () {
        const scrollTop = document.getElementById('scrolltop');
        if (scrollTop) {

            const fixNavbarTop = function () {
                const navbar = document.getElementById('header-nav');
                if (navbar === null || navbar === undefined) {
                    return;
                }

                navbar.classList.remove('fixed-top');
                navbar.classList.add('fixed-top');
            }

            const releaseNavbar = function () {
                const navbar = document.getElementById('header-nav');
                if (navbar === null || navbar === undefined) {
                    return;
                }

                navbar.classList.remove('fixed-top');
            }

            const toggleScrollTop = function () {

                if (window.scrollY > 100) {
                    scrollTop.classList.add('scrolltop_show');
                    fixNavbarTop();
                } else {
                    scrollTop.classList.remove('scrolltop_show');
                    releaseNavbar();
                }
              

              

            };
            window.addEventListener('load', toggleScrollTop);
            document.addEventListener('scroll', toggleScrollTop);
            scrollTop.addEventListener('click', function () {
                window.scrollTo({
                    top: 0,
                    behavior: 'smooth'
                });
            });

        }
    },
    initializeMasonry: function () {
        var elem = document.querySelector('.grid');
        if (elem) {
            imagesLoaded(elem, function () {
                new Masonry(elem, {
                    itemSelector: '.grid-item',
                    percentPosition: true,
                    horizontalOrder: true
                });
            });
        }
    },
    initializeBigPicture: function () {
        document.querySelectorAll("[data-bigpicture]").forEach((function (e) {
            e.addEventListener("click", (function (t) {
                t.preventDefault();
                const data = JSON.parse(e.dataset.bigpicture);
                BigPicture({
                    el: t.target,
                    ...data
                });
            }));
        }));
    },
    initializeGalleryPopup: function () {
        document.querySelectorAll(".bp-gallery a").forEach((function (e) {
            var caption = e.querySelector('figcaption');
            var img = e.querySelector('img');
            img.dataset.caption = '<a class="link-light" target="_blank" href="' + e.href + '">' + caption.innerHTML + '</a>';

            e.addEventListener("click", (function (t) {
                t.preventDefault();
                BigPicture({
                    el: t.target,
                    gallery: '.bp-gallery',
                });
            }));
        }));
    },
    initializeTheme: function () {

        let currentTheme = localStorage.getItem('theme');

        const queryParams = new URLSearchParams(window.location.search);
        const queryTheme = queryParams.get('t');

        // If a theme is specified in the query parameters and is equal 'dark' or 'light', use it
        if (queryTheme && queryTheme.length > 0) {
            currentTheme = queryTheme.toString().toLowerCase();
        }

        let nextTheme = 'light';

        if (currentTheme) {
            if (currentTheme === 'system') {
                if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
                    nextTheme = 'dark';
                } else if (window.matchMedia && window.matchMedia('(prefers-color-scheme: light)').matches) {
                    nextTheme = 'light';
                }
            }
            else if (currentTheme === 'dark' || currentTheme === 'light') {
                nextTheme = currentTheme;
            }
        } else if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            nextTheme = 'dark';
        } else if (window.matchMedia && window.matchMedia('(prefers-color-scheme: light)').matches) {
            nextTheme = 'light';
        }

        this.setTheme(nextTheme);
    },
    getTheme: function () {
        return localStorage.getItem('theme') || 'system';
    },
    toggleTheme: function () {
        let currentTheme = document.body.classList.contains('dark') ? 'dark' : 'light';
        let nextTheme = currentTheme === 'dark' ? 'light' : 'dark';

        this.setTheme(nextTheme);
    },
    setTheme: function (theme) {
        let currentTheme = document.body.classList.contains('dark') ? 'dark' : 'light';
        document.body.classList.remove(currentTheme);
        document.body.classList.add(theme);
        localStorage.setItem('theme', theme);

        // Update the Query parameters in the URL
        const url = new URL(window.location.href);
        if (theme === 'system') {
            url.searchParams.delete('t');
        } else {
            url.searchParams.set('t', theme);
        }
        window.history.replaceState({}, '', url.toString());
        this.setNavbarTheme();
    },
    setNavbarTheme: function () {
        const navbar = document.getElementById('header-nav');
        if (navbar === null || navbar === undefined) {
            return;
        }

        if (document.body.classList.contains('dark')) {
            navbar.classList.remove('navbar-light');
            navbar.classList.add('navbar-dark');
        } else {
            navbar.classList.remove('navbar-dark');
            navbar.classList.add('navbar-light');
        }
    },
    initializeLanguage: function () {

        let language = localStorage.getItem('language');

        // get language from query parameters if available
        const queryParams = new URLSearchParams(window.location.search);
        const queryLanguage = queryParams.get('l');

        if (queryLanguage && queryLanguage.length > 0) {
            language = queryLanguage.toString();
        }

        // if no language is set, use browser language
        if (!language || language.length === 0) {
            language = navigator.language || navigator.userLanguage;
        }

        if (!language || language.length === 0) { 
            language = navigator.languages.length > 0 ? navigator.languages[0] : null;
        }

        if (!language || language.length === 0) { 
            language = 'en-US';
        }

        localStorage.setItem('language', language);
        
        this.setBodyLanguage();

    },
    setBodyLanguage: function () {
        const language = localStorage.getItem('language') || 'en-US';
        document.documentElement.lang = language;
        document.documentElement.dir = (language === 'fa' || language === 'fa-IR' || language === 'ar') ? 'rtl' : 'ltr';

        // Update the Query parameters in the URL
        const url = new URL(window.location.href);
        url.searchParams.set('l', language);
        window.history.replaceState({}, '', url.toString());
    },
    getBrowserLanguages: function () {
        return navigator.languages;
    },
    getUrl: function () {
        return window.location.href;
    },
    initializeUi: function () {
        this.initializeScrollTop();
        this.initializeAOS();
        this.initializeMasonry();
        this.initializeBigPicture();
        this.initializeGalleryPopup();
        this.setNavbarTheme();
    },
    initialize: function () {
        this.initializeTheme();
        this.initializeLanguage();
    }
};
