function masonryReinitialize() {
	m = new Masonry('.grid', { fitWidth: true });
}

function masonryRedraw() {
	do {
		var x = document.getElementsByClassName("addtomasonry");
		if (x.length != 0) {
			let x0 = x[0];
			m.appended([x0]);
			x0.classList.remove("addtomasonry");
		}
		m.layout();
	} while (x.length != 0);
}
