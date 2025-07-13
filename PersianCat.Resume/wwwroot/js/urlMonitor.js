window.urlMonitor = {
    init: function (dotNetHelper) {
        if (window._urlMonitorInitialized) return;
        window._urlMonitorInitialized = true;

        const originalPushState = history.pushState;
        const originalReplaceState = history.replaceState;

        function notify() {
            dotNetHelper.invokeMethodAsync('OnUrlChanged', location.href);
        }

        history.pushState = function (...args) {
  
            originalPushState.apply(this, args);
            notify();
        };

        history.replaceState = function (...args) {
   
            originalReplaceState.apply(this, args);
            notify();
        };

        window.addEventListener('popstate', notify);
    },
    dispose: function () {
        if (!window._urlMonitorInitialized) return;
        window._urlMonitorInitialized = false;

        history.pushState = originalPushState;
        history.replaceState = originalReplaceState;
        window.removeEventListener('popstate', notify);
    }
};
