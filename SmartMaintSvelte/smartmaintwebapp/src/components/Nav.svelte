<script lang="ts">
	import type { Link } from '../definitions/interfaces.ts';
	const links: Link[] = [
		{ text: 'Home', href: '/' },
		{ text: 'About', href: '/about' }
	];
</script>

<ul>
	{#each links as { text, href }}
		<li>
			<a {href}>{text}</a>
		</li>
	{/each}
</ul>
