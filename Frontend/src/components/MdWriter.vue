<template>
  <div class="row">
    <div class="col s12">
      <h1>Markdown編輯器</h1>
    </div>
    <div class="input-field col s6">
      <textarea id="markdown-input" class="materialize-textarea"></textarea>
      <label for="markdown-input">Textarea</label>
    </div>
    <div id="markdown-display" class="col s6"></div>
  </div>
</template>

<script lang="ts">
import { onMounted, defineComponent } from "vue";
import markdownit from "markdown-it";
import hljs from "highlight.js";

let md: markdownit = new markdownit();

export default defineComponent({
  setup() {
    onMounted(async () => {
      md = markdownit({
        highlight: function (str, lang) {
          if (lang && hljs.getLanguage(lang)) {
            try {
              return (
                '<pre><code class="hljs">' +
                hljs.highlight(str, { language: lang, ignoreIllegals: true }).value +
                "</code></pre>"
              );
              // eslint-disable-next-line no-empty
            } catch (__) {}
          }

          return '<pre><code class="hljs">' + md.utils.escapeHtml(str) + "</code></pre>";
        },
      });
      const textarea = document.getElementById("markdown-input") as HTMLInputElement;
      const output = document.getElementById("markdown-display") as HTMLInputElement;

      textarea?.addEventListener("input", function () {
        const markdownText = textarea.value;
        const htmlContent = md.render(markdownText);
        output.innerHTML = htmlContent;

        hljs.highlightAll();
      });
    });

    return {};
  },
});
</script>
