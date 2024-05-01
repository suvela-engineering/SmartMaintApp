<script>
	import Nav from '../components/Nav.svelte';
	import Footer from '../components/Footer.svelte';
	import '../app.css';
</script>

<head>
	<meta charset="utf-8" />
	<title>SmartMaint</title>
	<meta name="description" content="A basic Svelte app example" />
</head>	

<body>
	<nav class="navigation">
		<Nav />
	</nav>
	<main class="main-content">
		<slot />
	</main>

	<footer class="footer">
		<Footer />
	</footer>
</body>
