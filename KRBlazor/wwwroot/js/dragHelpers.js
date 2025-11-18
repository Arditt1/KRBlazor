window.dragHelpers = (function () {
    const CELL_SIZE = 20;
    let currentInfo = null;
    let dragOffset = { x: 0, y: 0 };

    // Enable drag functionality for palette items
    function enableDrag() {
        const palette = document.querySelector(".palette-grid");
        if (!palette) {
            console.warn("Palette not found");
            return;
        }

        palette.addEventListener("dragstart", (e) => {
            const el = e.target.closest(".palette-item[draggable='true']");
            if (!el || !e.dataTransfer) return;

            const info = el.dataset.dragInfo;
            if (!info) return;

            const rect = el.getBoundingClientRect();
            dragOffset = { x: e.clientX - rect.left, y: e.clientY - rect.top };

            currentInfo = info;
            e.dataTransfer.clearData();
            e.dataTransfer.setData("text/plain", info);
            e.dataTransfer.setData("offset-x", dragOffset.x);
            e.dataTransfer.setData("offset-y", dragOffset.y);
            e.dataTransfer.effectAllowed = "copy";
        });

        palette.addEventListener("dragend", () => {
            currentInfo = null;
            dragOffset = { x: 0, y: 0 };
        });
    }

    // Initialize drop zones on the anchorage board
    function initDropZones(dotnetRef) {
        const board = document.getElementById("anchorage-board");
        if (!board) {
            console.warn("Anchorage board not found");
            return;
        }

        board.addEventListener("dragover", (e) => {
            e.preventDefault();
        });

        board.addEventListener("drop", (e) => {
            e.preventDefault();

            const info = e.dataTransfer?.getData("text/plain") || currentInfo;
            if (!info) return;

            const rect = board.getBoundingClientRect();
            const offsetX = parseFloat(e.dataTransfer?.getData("offset-x")) || dragOffset.x;
            const offsetY = parseFloat(e.dataTransfer?.getData("offset-y")) || dragOffset.y;

            const x = Math.floor((e.clientX - rect.left - offsetX) / CELL_SIZE);
            const y = Math.floor((e.clientY - rect.top - offsetY) / CELL_SIZE);

            currentInfo = null;
            dragOffset = { x: 0, y: 0 };

            // Notify Blazor about the drop event
            if (dotnetRef) {
                dotnetRef.invokeMethodAsync("HandleDrop", info, x, y);
            } else {
                console.warn("DotNet reference is null, drop not handled.");
            }
        });
    }

    return {
        enableDrag,
        initDropZones
    };
})();
