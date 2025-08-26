<template>
  <nav>
    <div class="nav-wrapper">
      <router-link to="/" class="brand-logo">Home</router-link>
      <a href="#" data-target="mobile-demo" class="sidenav-trigger"><i class="material-icons">menu</i></a>
      <!-- common -->
      <ul class="right hide-on-med-and-down">
        <li>
          <router-link to="/">
            首頁
            <i class="material-icons left">home</i>
          </router-link>
        </li>
        <li>
          <router-link to="/folder">
            檔案夾
            <i class="material-icons left">folder_open</i>
          </router-link>
        </li>
        <li>
          <router-link to="/markdown-writer">
            文字編輯器
            <i class="material-icons left">edit_note</i>
          </router-link>
        </li>
      </ul>
    </div>
  </nav>
  <!-- mobile -->
  <ul class="sidenav" id="mobile-demo">
    <li>
      <router-link to="/">首頁</router-link>
    </li>
    <li>
      <router-link to="/folder">檔案夾</router-link>
    </li>
    <li>
      <router-link to="/markdown-writer">文字編輯器</router-link>
    </li>
  </ul>
</template>

<script setup lang="ts">
import { onMounted } from "vue";
import { initMaterialSidenav } from "@/composables/useMaterial";

onMounted(() => {
  initMaterialSidenav();
});
</script>

<style scoped>
.nav-wrapper {
  background-color: #f2ebea;
}
.brand-logo {
  margin-left: 10px;
}
a {
  color: #444444 !important;
}
</style>
