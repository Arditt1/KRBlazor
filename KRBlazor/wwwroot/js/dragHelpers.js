window.dragHelpers = (function () {
    const CELL_SIZE = 20; 
    let currentInfo = null;
    let dragOffset = { x: 0, y: 0 }; 

    return {
        getElementWidth: function (id) {
            const el = document.getElementById(id);
            return el ? el.clientWidth : 0;
        },

        enableDrag: function () {

            document.addEventListener(
                "dragstart",
                (e) => {
                    const el = e.target.closest(".palette-item[draggable='true']");
                    if (!el || !e.dataTransfer) return;

                    const info = el.dataset.dragInfo;
                    if (!info) return;

                    const rect = el.getBoundingClientRect();
                    dragOffset = {
                        x: e.clientX - rect.left,
                        y: e.clientY - rect.top,
                    };

                    currentInfo = info;
                    e.dataTransfer.clearData();
                    e.dataTransfer.setData("text/plain", info);
                    e.dataTransfer.setData("offset-x", dragOffset.x);
                    e.dataTransfer.setData("offset-y", dragOffset.y);

                    e.dataTransfer.effectAllowed = "copy";
                    e.dataTransfer.dropEffect = "copy";

                },
                true
            );

            document.addEventListener(
                "dragend",
                () => {
                    currentInfo = null;
                    dragOffset = { x: 0, y: 0 };
                },
                true
            );
        },

        initDropZones: function (dotnetRef) {

            const board = document.getElementById("anchorage-board");
            if (!board) {
                return;
            }

            board.addEventListener("dragover", (e) => {
                e.preventDefault();
                if (e.dataTransfer) e.dataTransfer.dropEffect = "copy";
            });

            board.addEventListener("drop", (e) => {
                e.preventDefault();

                const info =
                    (e.dataTransfer && e.dataTransfer.getData("text/plain")) || currentInfo;
                if (!info) {
                    return;
                }

                const rect = board.getBoundingClientRect();

                const offsetX =
                    parseFloat(e.dataTransfer?.getData("offset-x")) || dragOffset.x || 0;
                const offsetY =
                    parseFloat(e.dataTransfer?.getData("offset-y")) || dragOffset.y || 0;

                const x = Math.floor((e.clientX - rect.left - offsetX) / CELL_SIZE);
                const y = Math.floor((e.clientY - rect.top - offsetY) / CELL_SIZE);


                currentInfo = null;
                dragOffset = { x: 0, y: 0 };

                dotnetRef.invokeMethodAsync("HandleDrop", info, x, y);
            });
        },
    };
})();
