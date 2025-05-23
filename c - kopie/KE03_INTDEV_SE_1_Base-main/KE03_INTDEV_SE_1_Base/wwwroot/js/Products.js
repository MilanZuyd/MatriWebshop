document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.add-to-cart-form').forEach(form => {
        form.addEventListener('submit', function (e) {
            e.preventDefault();

            const formData = new FormData(form);
            const url = form.getAttribute('action') || window.location.pathname + '?handler=AddToCart';

            fetch(url, {
                method: 'POST',
                body: formData,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest'
                }
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        showToast("Product added to shopping cart!");
                    } else {
                        showToast("Something went wrong when adding poduct.", true);
                    }
                })
                .catch(() => {
                    showToast("An error has occured", true);
                });
        });
    });

    function showToast(message, isError = false) {
        const toast = document.createElement('div');
        toast.className = `toast align-items-center text-white ${isError ? 'bg-danger' : 'bg-success'} border-0 position-fixed bottom-0 end-0 m-4`;
        toast.style.zIndex = 1055;
        toast.setAttribute('role', 'alert');
        toast.setAttribute('aria-live', 'assertive');
        toast.setAttribute('aria-atomic', 'true');
        toast.innerHTML = `
            <div class="d-flex">
                <div class="toast-body">
                    ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        `;
        document.body.appendChild(toast);
        const bootstrapToast = new bootstrap.Toast(toast, { delay: 3000 });
        bootstrapToast.show();

        toast.addEventListener('hidden.bs.toast', () => {
            toast.remove();
        });
    }
});
