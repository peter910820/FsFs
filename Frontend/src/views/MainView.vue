<template>
  <div class="main-style">
    <div class="sticky-navbar">
      <NavBar />
    </div>
    <div class="row">
      <div class="col m12 m12 s12">
        <router-view />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

import NavBar from "@/components/NavBar.vue";

export default defineComponent({
  components: {
    NavBar,
  },
  setup() {
    return;
  },
});
</script>

<style>
.main-style {
  min-height: 100vh;
  min-width: 600px;
  font-family: "madoufmg";

  background-color: #f2ebea;
  background-size: 400% 400%;
}

.row {
  height: 100%;
  margin-bottom: 0px !important;
  padding: 30px;
}
.main-block {
  padding: 40px;
}
.sub-block {
  padding-top: 20px;
  margin-top: 10px;
  cursor: default !important;
  border: 2px solid f2ebea;
  border-radius: 20px;
}
.sticky-navbar {
  position: sticky;
  top: 0px;
  z-index: 10;
}
.browser-default {
  background-color: #f2ebea;
  border: 0px;
  border-bottom: 1px solid rgb(118, 118, 118);
}
/* hide materializecss select in mobile */
.mobile-hidden {
  display: block !important;
}
.mobile-display {
  display: none !important;
}
@media (max-width: 768px) {
  .mobile-display {
    display: block !important;
  }
  .mobile-hidden {
    display: none !important;
  }
}
/* font-settings */
@font-face {
  font-family: "madoufmg";
  src: url("@/assets/fonts/madoufmg.ttf") format("truetype");
}
</style>
