window.dragModule = (() => {
    const GRID_UNIT = 20;
    let dragPayload = null;
    let grabPoint = null;

    function findDraggable(target) {
        return target.closest("[data-vessel]");
    }

    function onDragStart(e) {
        const element = findDraggable(e.target);
        if (!element || !e.dataTransfer) return;

        dragPayload = element.dataset.vessel;

        const box = element.getBoundingClientRect();
        grabPoint = {
            dx: e.clientX - box.left,
            dy: e.clientY - box.top
        };

        e.dataTransfer.effectAllowed = "copyMove";
        e.dataTransfer.setData("payload", dragPayload);
    }

    function onDragEnd() {
        dragPayload = null;
        grabPoint = null;
    }

    function setupWorkspace(dotNetRef) {
        const grid = document.querySelector("#anchorage-board");
        if (!grid) return;

        grid.addEventListener("dragover", evt => {
            evt.preventDefault();
            if (evt.dataTransfer) evt.dataTransfer.dropEffect = "copy";
        });

        grid.addEventListener("drop", evt => {
            evt.preventDefault();

            const payload = evt.dataTransfer?.getData("payload") || dragPayload;
            if (!payload) return;

            const base = grid.getBoundingClientRect();
            const dx = grabPoint?.dx ?? 0;
            const dy = grabPoint?.dy ?? 0;

            const gx = Math.floor((evt.clientX - base.left - dx) / GRID_UNIT);
            const gy = Math.floor((evt.clientY - base.top - dy) / GRID_UNIT);

            dragPayload = null;
            grabPoint = null;

            dotNetRef.invokeMethodAsync("OnDrop", payload, gx, gy);
        });
    }

    function init() {
        console.log('Drag module initialized');
        document.addEventListener("dragstart", onDragStart);
        document.addEventListener("dragend", onDragEnd);
    }

    return {
        init: init,
        enableDrop: setupWorkspace
    };
})();
