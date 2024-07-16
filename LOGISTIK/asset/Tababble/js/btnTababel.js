function openTab(tabId) {
    var tabContents = document.getElementsByClassName('tab-content')
    for (var i = 0; i < tabContents.length; i++) {
        tabContents[i].classList.remove('show')
    }

    document.getElementById(tabId).classList.add('show')
}