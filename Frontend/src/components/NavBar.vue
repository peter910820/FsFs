<script setup lang="ts">
import { onMounted } from "vue";
import { initMaterialSidenav, initMaterialModal } from "@/composables/useMaterial";
import { useLoginStore } from "@/store/login";

const loginStatus = useLoginStore();

onMounted(() => {
  initMaterialSidenav();
  initMaterialModal();
});
</script>

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
        <li v-if="!loginStatus.status">
          <router-link to="" class="modal-trigger" data-target="modal1">
            登入
            <i class="material-icons left">key</i>
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
    <li v-if="!loginStatus.status">
      <router-link to="" class="modal-trigger" data-target="modal1">登入</router-link>
    </li>
  </ul>
</template>

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
