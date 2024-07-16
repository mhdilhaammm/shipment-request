$(".delete-button").click(function (e) {
    e.preventDefault();
    var deleteForm = $(this).closest("form");

    Swal.fire({
        title: 'Are You Sure?',
        text: 'You Canot see this data again!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'yes, deleted!',
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: 'Success!',
                text: 'The data has been deleted successfully.',
                icon: 'success',
                showConfirmButton: false
            });
            setTimeout(function () {
                Swal.close(); // Menutup popup Swal setelah 2 detik
                deleteForm.submit(); // Kirim form saat tombol "Ya, hapus!" ditekan
            }, 1500); // Waktu dalam milisekon
        }
        //DELETE: pop up success delete 
    });
});

// sweetalert success
showSuccessAlert = (message) => {
    Swal.fire({
        title: 'Success',
        icon: 'success',
        text: 'New Account Successfully Created',
        showConfirmButton: false,
        timer: 1000
    })
}