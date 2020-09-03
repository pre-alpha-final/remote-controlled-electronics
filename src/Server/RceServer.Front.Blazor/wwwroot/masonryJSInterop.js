function masonryReinitialize() {
	m = new Masonry('.grid', { fitWidth: true });
}

function masonryRedraw() {
	var x = document.getElementsByClassName("addtomasonry");
	for (i = x.length; i > 0; i--) {
		m.appended([x[i - 1]]);
		x[i-1].classList.remove("addtomasonry");
	}
	m.layout();
}
