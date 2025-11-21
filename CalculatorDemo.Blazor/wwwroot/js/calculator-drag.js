// Lightweight drag support for the calculator shell so it can be repositioned.
(function () {
    const padding = 12; // keep the window away from the very edge

    const clamp = (left, top, width, height) => {
        const maxLeft = Math.max(window.innerWidth - width - padding, padding);
        const maxTop = Math.max(window.innerHeight - height - padding, padding);
        return {
            left: Math.min(Math.max(left, padding), maxLeft),
            top: Math.min(Math.max(top, padding), maxTop)
        };
    };

    const placeInitially = (element) => {
        const rect = element.getBoundingClientRect();
        const preferredLeft = (window.innerWidth - rect.width) / 2;
        const preferredTop = Math.max((window.innerHeight - rect.height) / 3, padding * 2);
        const clamped = clamp(preferredLeft, preferredTop, rect.width, rect.height);
        element.style.position = "fixed";
        element.style.left = `${clamped.left}px`;
        element.style.top = `${clamped.top}px`;
        element.style.transform = "none";
    };

    const attachResizeGuard = (element) => {
        const handler = () => {
            const rect = element.getBoundingClientRect();
            const clamped = clamp(rect.left, rect.top, rect.width, rect.height);
            element.style.left = `${clamped.left}px`;
            element.style.top = `${clamped.top}px`;
        };

        window.addEventListener("resize", handler);
    };

    const attachDrag = (element) => {
        const handle = element.querySelector(".calc-header") || element;
        let offsetX = 0;
        let offsetY = 0;
        let dragging = false;

        const onPointerMove = (event) => {
            if (!dragging) {
                return;
            }

            const rect = element.getBoundingClientRect();
            const desiredLeft = event.clientX - offsetX;
            const desiredTop = event.clientY - offsetY;
            const clamped = clamp(desiredLeft, desiredTop, rect.width, rect.height);
            element.style.left = `${clamped.left}px`;
            element.style.top = `${clamped.top}px`;
        };

        const onPointerUp = (event) => {
            if (!dragging) {
                return;
            }

            dragging = false;
            element.classList.remove("is-dragging");
            handle.releasePointerCapture?.(event.pointerId);
            window.removeEventListener("pointermove", onPointerMove);
        };

        const onPointerDown = (event) => {
            if (event.button !== 0) {
                return;
            }

            const rect = element.getBoundingClientRect();
            dragging = true;
            element.classList.add("is-dragging");
            offsetX = event.clientX - rect.left;
            offsetY = event.clientY - rect.top;
            handle.setPointerCapture?.(event.pointerId);
            window.addEventListener("pointermove", onPointerMove);
            window.addEventListener("pointerup", onPointerUp, { once: true });
            event.preventDefault();
        };

        handle.style.cursor = "grab";
        handle.addEventListener("pointerdown", onPointerDown);
    };

    window.calculatorDrag = {
        enableDrag: (element) => {
            if (!element || element.dataset.dragEnabled === "true") {
                return;
            }

            element.dataset.dragEnabled = "true";
            placeInitially(element);
            attachResizeGuard(element);
            attachDrag(element);
        }
    };
})();
