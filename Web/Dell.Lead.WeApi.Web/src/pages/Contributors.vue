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
              <h5>CPF</h5>
              <InputMask id="cpf" v-model="cpf" mask="999.999.999-99" placeholder="000.000.000-00"/>
            </span>
            <span class="p-field">
              <h5>name</h5>
              <InputText id="name_full" type="text" v-model="name" />
            </span>
            <span class="p-field">
              <h5>Date_of_Birth</h5>
              <Calendar id="Date_of_Birth" v-model="Date_of_Birth"  dateFormat="mm-dd-yy" />
            </span>
            <span class="p-field">
              <h5>Phone</h5>
              <InputMask id="phone" mask="(99)9.9999-9999" placeholder="(99)9.9999-9999" v-model="Cellfone" />
            </span>
            <span class="p-field">
              <h5>Gender</h5>
              <InputText id="gender" type="text" v-model="gender" />
            </span>
            <span class="p-field">
              <h5>CEP</h5>
              <InputText id="cep" v-model="cep" @keyup.enter="searchCep" />
            </span>
            <span class="p-field">
              <h5>Street</h5>
              <InputText id="street" type="text" v-model="viaCep.street" />
            </span>
            <span class="p-field">
              <h5>Number</h5>
              <InputNumber id="number" v-model="number" />
            </span>
            <span class="p-field">
              <h5>District</h5>
              <InputText id="district" type="text" v-model="district" />
            </span>
            <span class="p-field">
              <h5>City</h5>
              <InputText id="city" type="text" v-model="city" />
            </span>
            <span class="p-field">
              <h5>State</h5>
              <InputMask id="state" mask="**" v-model="viaCep.state" />
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
        <Column field="code" header="ID" :sortable="true"></Column>
        <Column 
          field="name" 
          filterField="name"
          :sortable="true" 
          :showFilterMatchModes="false"
          header="Nome"
        >
          <template #filter="{filterModel}">
              <div class="mb-3 font-bold">Nome</div>
              <InputText v-model="filterModel.value"/>
          </template>
        </Column>
        <Column 
          field="cpf" 
          header="CPF"
          filterField="cpf" 
          :showFilterMatchModes="false"  
        >
          <template #filter="{filterModel}">
              <div class="mb-3 font-bold">CPF</div>
              <InputText v-model="filterModel.value"/>
          </template>
        </Column>
        <Column field="Date_of_Birth" header="Data de nascimento"></Column>
        <Column field="Cellfone" header="Telefone"></Column>
        <Column field="gender" header="Gênero"></Column>
        <Column field="address.street" header="Rua"></Column>
        <Column field="address.number" header="Número"></Column>
        <Column field="address.district" header="Bairro"></Column>
        <Column field="address.city" header="Cidade"></Column>
        <Column field="address.state" header="Estado"></Column>
        <Column field="address.cep" header="CEP"></Column>
        <Column :exportable="false">
          <template #body="slotProps">
            <Button icon="pi pi-pencil" class="p-button-rounded p-button-success p-mr-2" @click="editContributors(slotProps.data)" />
            <Button icon="pi pi-trash" class="p-button-rounded p-button-warning" @click="confirmContributors(slotProps.data.id)" />
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
import ContributorsService from '../services/ContributorsService'
import ViaCepService from '../services/ViaCepService'

export default {
  name: 'Contributors',
  data() {
    return {
      contributors: null,
      contributors: [],
      viaCep: {
        street: ''
      },
      id: null,
      name: '',
      cpf: null,
      date_of_birth: '',
      cellfone: null,
      gender: '',
      isSave: true,
      Address: [],
      filters: []
    }
  },
  ContributorsService: null,
  created(){
    this.ContributorsService = new ContributorsService();
    this.viaCepService = new ViaCepService();
    this.filters = this.initFilters();
  },
  async mounted(){
    await this.requestGetyContributors();
  },
  methods: {
  initFilters() {
    return {
      'name': { value: null, matchMode: FilterMatchMode.CONTAINS },
      'cpf': { value: null, matchMode: FilterMatchMode.CONTAINS },
    };
  },
  showSuccess() {
      this.$toast.add({severity:'success', summary: 'Success Message', detail:'Message Content', life: 3000});
  },
  showError() {
      this.$toast.add({severity:'error', summary: 'Error Message', detail:'Message Content', life: 3000});
  },
  async searchCep(){
    await this.requestGetViaCep(this.cep);
    console.log(this.cep);
    console.log(this.viaCep);
  },
  async requestGetyContributors(){
    await this.ContributorsService.getContributors()
      .then(response => {
        let data = response.data;
        this.Contributors = [];
        data.forEach(Contributors => {
          this.Contributors.push({...Contributors})
        });
      })
      .catch(() => {
        console.log('Have one Mistake!');
      });
  },
  async requestGetViaCep(cep){
    await this.viaCepService.getViaCep(cep)
      .then(response => {
        let data = {...response.data};
        this.viaCep = {
          city: data.localidade,
          district: data.bairro,
          street: data.logradouro,
          state: data.uf
        };
      })
      .catch(() => {
        console.log('Have one Mistake!')
      });
  },
  async save(cpf){
  let contributors = {
      'name': this.name,
      'cpf': this.cpf,
      'date_of_birth': this.date_of_birth,
      'cellfone': this.cellfone,
      'gender': this.gender,
    };
    employee.Address = {
      'street': this.viaCep.street,
      'district': this.viaCep.district,
      'city': this.viaCep.city,
      'state': this.viaCep.state,
      'number': this.number,
      'cep': this.cep
      };
      this.clearField();
      await this.requestPostContributors(contributors);
  },
   async requestGetViaCep(cep){
    await this.viaCepService.getViaCep(cep)
      .then(response => {
        let data = {...response.data};
        this.viaCep = {
          city: data.localidade,
          district: data.bairro,
          street: data.logradouro,
          state: data.uf
        };
      })
    },
     async requestPutContributors(Contributors = {}){
      await this.ContributorsService.putContributors(Contributors)
        .then(() => {
          this.requestGetContributors();
          this.showSuccess();
        })
        .catch(() => {
          this.showError();
        });
    },
    async put(){
      let contributors = {
      'code': this.id, 
      'name': this.name,
      'cpf': this.cpf,
      'date_of_birth': this.date_of_birth,
      'cellfone': this.cellfone,
      'gender': this.gender,
    };

    }
  }
}
</script>