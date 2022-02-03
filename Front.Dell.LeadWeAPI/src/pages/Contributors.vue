<template>
	<Toast />
	<Dialog header="Confirmation" v-model:visible="displayConfirmation" :style="{width: '350px'}" :modal="true">
		<div class="confirmation-content">
			<i class="pi pi-exclamation-triangle p-mr-3" style="font-size: 2rem" />
			<span>Are you sure you want to proceed?</span>
		</div>
		<template #footer>
			<Button label="No" icon="pi pi-times" @click="cancelContributors" class="p-button-text"/>
			<Button label="Yes" icon="pi pi-check" @click="deleteContributors" class="p-button-text" autofocus />
		</template>
	</Dialog>
  <div class="content">
    <div class="card-content">
      <Card>
        <template #title>
          <h1>Contributors</h1>
        </template>
        <template #content> 
          <div class="content-filter">
            <span class="p-field">
              <h5>Name</h5>
              <InputText id="name" type="text" v-model="name" disabled />
            </span>
            <span class="p-field">
              <h5>Cpf</h5>
              <InputNumber id="Cpf" type="text" v-model="Cpf" />
            </span>
           <span class="p-field">
              <h5>Cellfone</h5>
              <InputNumber id="Cellfone" type="text" v-model="Cellfone" disabled />
            </span>
             <span class="p-field">
              <h5>Gender</h5>
              <InputText id="Gender" type="text" v-model="Gender" disabled />
            </span>
              <h5>Address</h5>
              <!--<Dropdown 
                v-model="selectedAuthor" 
                :options="optionsAuthors" 
                optionLabel="name"  
                placeholder="Select a Author" 
              />-->
              <Dropdown 
								v-model="selectedAddress" 
								:options="optionsAddress" 
								optionLabel="name" 
								:filter="true" 
								placeholder="Select a Address" 
								:showClear="true"
							>
								<template #value="slotProps">
									<div class="country-item country-item-value" v-if="slotProps.value">
										<div>{{slotProps.value.name}}</div>
									</div>
								<span v-else>
										{{slotProps.placeholder}}
									</span>
								</template>
								<template #option="slotProps">
									<div class="country-item">
										<div>{{slotProps.option.name}}</div>
									</div>
								</template>
            </Dropdown>
            </span>
          </div> 
          <div class="d-button">
            <Button label="Save" @click="save" v-if="isSave"/>
            <Button label="Update" @click="update" v-else/>
          </div>    
        </template>
      </Card>
    </div>
    
    <div class="d-datatable">
      <DataTable 
        :value="Contributors" 
        :scrollable="true" 
        scrollHeight="400px" 
        responsiveLayout="scroll"
        filterDisplay="menu"
        v-model:filters="filters"
      >
        <Column field="id" header="ID" :sortable="true"></Column>
        <Column 
          field="title" 
          filterField="title"
          :sortable="true" 
          :showFilterMatchModes="false"
          header="Title"
        >
          <template #filter="{filterModel}">
              <div class="mb-3 font-bold">Cpf</div>
              <InputText v-model="filterModel.value"/>
          </template>
        </Column>
        <Column 
          field="cpf" 
          header="cof"
          filterField="cpf" 
          :showFilterMatchModes="false"  
        >
          <template #filter="{filterModel}">
              <div class="mb-3 font-bold">Price</div>
              <InputText v-model="filterModel.value"/>
          </template>
        </Column>
        <Column field="author.name" header="Author"></Column>
        <Column :exportable="false">
          <template #body="slotProps">
            <Button icon="pi pi-pencil" class="p-button-rounded p-button-success p-mr-2" @click="editContributors(slotProps.data)" />
            <Button icon="pi pi-trash" class="p-button-rounded p-button-warning" @click="confirmDeleteContributors(slotProps.data.id)" />
          </template>
        </Column>
        <template #empty>
            No records found.
        </template>
      </DataTable>
    </div>
  </div>
</template>

<script>
import { FilterMatchMode } from 'primevue/api';
import ContributorsService from '../services/ContributorsService';
import AddressService from '../services/AddressService';
export default {
  name: 'Contributors',
  data() {
    return {
      Contributors: null,
      id: null,
      name: '',
      cpf: null,
      Contributors: [],
      optionsAddress: [],
      selectedAddress:null,
      isSave: true,
      displayConfirmation: false,
      filters: []
    }
  },
  bookService: null,
  authorService: null,
  created() {
    this.ContributorsService = new ContributorsService();
    this.AddressService = new AddressService();
    this.filters = this.initFilters();
  },
  async mounted() {
    await this.requestGetAddress();
    await this.requestGetContributors();
  },
  methods: {
    initFilters() {
      return {
        'Name': { value: null, matchMode: FilterMatchMode.CONTAINS },
        'Cpf': { value: null, matchMode: FilterMatchMode.CONTAINS },
      };
    },
    showSuccess() {
        this.$toast.add({severity:'success', summary: 'Success Message', detail:'Message Content', life: 3000});
    },
    showError() {
        this.$toast.add({severity:'error', summary: 'Error Message', detail:'Message Content', life: 3000});
    },
    async save() {
      let contributors = {
        'name': this.name,
        'cpf': this.cpf
      };
      if(this.selectedAddress) {
        contributors.address = {
          'id': this.selectedAddress.id
        };
      }
      await this.requestPostBook(contributors);
      this.clearField();
    },
    async update() {
     let contributors = {
        'name': this.name,
        'cpf': this.cpf
      };
      if(this.selectedAddress) {
        contributors.andress = {
          'id': this.selectedAddress.id
        };
      }
      await this.requestPutContributos(this.id, contributors);
      this.clearField();
      this.isSave = true;
    },
    async deleteContributos(){
      await this.requestDeleteContributos(this.id);
      this.displayConfirmation = false;
      this.clearField();
      this.isSave = true;
    },
    cancelContributos() {
      this.id= null;
      this.displayConfirmation = false;
      this.clearField();
      this.isSave = true;
    },
    clearField() {
      this.id = null,
      this.name = '',
      this.cpf = null,
      this.selectedAddress = null
    },
    editContributos(data) {
      this.name = data.name;
      this.cpf = data.cpf;
      this.id = data.id;
      let author = {...data.author};
      if(author.name !== '-') {
        this.selectedAddress = {
            id: andress.id,
            name: address.name    
        };
      } else {
          this.selectedAddress = null;
      }
      this.isSave = false;
    },
    confirmDeleteContributos(value) {
      this.id = value;
      this.displayConfirmation = true;
    },
    async requestGetContributos() {
      await this.ContributosService.getContributos()
        .then(response => {
          let data = response.data;
          this.Contributos = [];
          data.forEach(Contributos => {
            let ContributosLocal = {...Contributos};
            ContributosLocal.address =  ContributosLocal.address ? ContributosLocal.address : { name: '-'};
            this.Contributos.push({...ContributosLocal});
          });
        })
        .catch(() => {
          console.log('Ocorreu um erro!');
        });
    },
    async requestGetContributos() {
      await this.ContributosService.getContributos()
        .then(response => {
          let data = response.data;
          this.optionsAddress = [];
          data.forEach(address => {
            this.optionsAddress.push({
              id: address.id,
              name: address.name
            })
          });
        })
        .catch(() => {
          this.optionsAddress=[];
          this.showError();
        })
    },
    async requestPostcontributors(contributors = {}) {
      await this.contributorsService.postcontributors(contributors)
        .then(() => {
          this.requestGetcontributors();
          this.showSuccess();
        })
        .catch(error => {
          this.$toast.add({severity:'error', summary: 'Error Register', detail:`${error.response.data.name}`, life: 3000});
        })
    },
    async requestPutBook(contributorsId, contributors = {}) {
      await this.contributorsService.putcontributors(contributorsId, contributors)
        .then(() => {
          this.requestGetcontributors();
          this.showSuccess();
        })
        .catch(() => {
          this.showError();
        })
    },
    async requestDeletecontributors(contributorsId) {
      await this.contributorsService.deleteBooks(contributorsId)
        .then(() => {
          this.requestGetcontributors();
          this.showSuccess();
        })
        .catch(() => {
          this.showError();
        })
    }
  }
}
</script>