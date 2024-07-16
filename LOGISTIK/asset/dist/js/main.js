window.addEventListener('load', function (){
    const loader = document.getElementById('loading')
    setTimeout(function () {
        loader.style.opacity = '0'
        setTimeout(function () {
            loader.style.display = 'none'
        }, 800)
    }, 500)
})