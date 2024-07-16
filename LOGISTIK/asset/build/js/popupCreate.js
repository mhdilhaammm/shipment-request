@section Scripts {
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@10"></script>
    <script>
        $(document).ready(function(){
            var successMessage = '@TempData["SuccessMessage"]';
            if (successMessage) {
                Swal.fire({
                    title: 'Success!',
                    text: successMessage,
                    icon: 'success'
                });
            }
        });
    </script>
}
