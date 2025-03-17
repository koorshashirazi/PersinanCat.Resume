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

    handleScroll: function () {
        const navbar = document.getElementById('header-nav');
        var body = document.getElementsByTagName("body")[0];
        const scrollTop = document.getElementById('scrolltop');

        window.onscroll = () => {
            if (window.scrollY > 0) {
                navbar.classList.add('fixed-top', 'shadow-sm');
                body.style.paddingTop = navbar.offsetHeight + "px";
                scrollTop.style.visibility = "visible";
                scrollTop.style.opacity = 1;
            } else {
                navbar.classList.remove('fixed-top', 'shadow-sm');
                body.style.paddingTop = "0px";
                scrollTop.style.visibility = "hidden";
                scrollTop.style.opacity = 0;
            }
        };
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
    }
};
