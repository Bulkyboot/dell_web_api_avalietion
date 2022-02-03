<template>
  <div class="d-menubar">
    <Menubar :model="items">
      <template #start>
        <img alt="logo" src="https://www.primefaces.org/wp-content/uploads/2020/05/placeholder.png" height="40" class="p-mr-2">
      </template>
    </Menubar>
  </div>
  <router-view />
  <!--<template v-if="isContributors">
    <Contributors />
  </template>
  <template v-else>
    <Address />
  </template>-->
</template>

<script>
//import Contributors from './pages/Contributors.vue';
import items from './data/menu';
//import Address from './pages/Address.vue';

  export default {
    name: 'App',
    // components: {
    //   Contributors//,
    //   //Address
    // },
    data() {
      return {
        items,
        isContributors: false
      }
    }
  }
</script>

<style lang="scss">
  body {
    background-color: #edf0f5;
    margin: 0;
  }
#app {
  .d-menubar {
    .p-menubar {
      border: none;
      border-radius: 0;
      background: linear-gradient(90deg,#0388e5 0,#07bdf4);
    }
  }
}
</style>
