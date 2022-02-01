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
              <h5>name</h5>
              <InputText id="name" type="text" v-model="name" disabled />
            </span>
            <span class="p-field">
              <h5>cpf</h5>
              <InputNumber id="cpf" type="text" v-model="cpf" />
            </span>
            <span class="p-field">
              <h5>DateOfBirth</h5>
              <InputNumber id="dateOfBirth" v-model="DateOfBirth" />
            </span>
            <span class="p-field">
              <h5>cellfone</h5>
                <InputNumber id="cellfone" v-model="cellfone" />
            </span>
            <span class="p-field">
              <h5>gener</h5>
                <InputText id="gener" v-model="gener"/>
            </span>
            <span class="p-field">
              <h5>andress</h5>
              <!--<Dropdown 
                v-model="selectedandress" 
                :options="optionsandresss" 
                optionLabel="name"  
                placeholder="Select a andress" 
              />-->
              <Dropdown 
								v-model="selectedandress" 
								:options="optionsandress" 
								optionLabel="Cep" 
								:filter="true" 
								placeholder="Select a Contrubutirs" 
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
      <DataTable :value="Contributors" :scrollable="true" scrollHeight="400px" responsiveLayout="scroll">
        <Column field="id" header="ID"></Column>
        <Column field="title" header="Title"></Column>
        <Column field="price" header="Price"></Column>
        <Column field="author.name" header="Author"></Column>
        <Column :exportable="false">
          <template #body="slotProps">
            <Button icon="pi pi-pencil" class="p-button-rounded p-button-success p-mr-2" @click="editBook(slotProps.data)" />
            <Button icon="pi pi-trash" class="p-button-rounded p-button-warning" @click="confirmDeleteBook(slotProps.data.id)" />
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
import Contributor from '../services/BookService';
import AuthorService from '../services/AuthorService';

export default {
  name: 'Book',
  data() {
    return {
      book: null,
      id: null,
      title: '',
      price: null,
      books: [],
      optionsAuthors: [],
      selectedAuthor:null,
      isSave: true,
      displayConfirmation: false
    }
  },
  bookService: null,
  authorService: null,
  created() {
    this.bookService = new BookService();
    this.authorService = new AuthorService();
  },
  async mounted() {
    await this.requestGetAuthors();
    await this.requestGetBooks();
  },
  methods: {
    showSuccess() {
        this.$toast.add({severity:'success', summary: 'Success Message', detail:'Message Content', life: 3000});
    },
    showError() {
        this.$toast.add({severity:'error', summary: 'Error Message', detail:'Message Content', life: 3000});
    },
    async save() {
      let book = {
        'title': this.title,
        'price': this.price
      };
      if(this.selectedAuthor) {
        book.author = {
          'id': this.selectedAuthor.id
        };
      }
      await this.requestPostBook(book);
      this.clearField();
    },
    async update() {
      let book = {
        'title': this.title,
        'price': this.price
      };
      if(this.selectedAuthor) {
        book.author = {
          'id': this.selectedAuthor.id
        };
      }
      await this.requestPutBook(this.id, book);
      this.clearField();
      this.isSave = true;
    },
    async deleteBook(){
      await this.requestDeleteBook(this.id);
      this.displayConfirmation = false;
      this.clearField();
      this.isSave = true;
    },
    cancelBook() {
      this.id= null;
      this.displayConfirmation = false;
      this.clearField();
      this.isSave = true;
    },
    clearField() {
      this.id = null,
      this.title = '',
      this.price = null,
      this.selectedAuthor = null
    },
    editBook(data) {
      this.title = data.title;
      this.price = data.price;
      this.id = data.id;
      let author = {...data.author};
      if(author.name !== '-') {
        this.selectedAuthor = {
            id: author.id,
            name: author.name    
        };
      } else {
          this.selectedAuthor = null;
      }
      this.isSave = false;
    },
    confirmDeleteBook(value) {
      this.id = value;
      this.displayConfirmation = true;
    },
    async requestGetBooks() {
      await this.bookService.getBooks()
        .then(response => {
          let data = response.data;
          this.books = [];
          data.forEach(book => {
            let bookLocal = {...book};
            bookLocal.author =  bookLocal.author ? bookLocal.author : { name: '-'};
            this.books.push({...bookLocal});
          });
        })
        .catch(() => {
          console.log('Ocorreu um erro!');
        });
    },
    async requestGetAuthors() {
      await this.authorService.getAuthors()
        .then(response => {
          let data = response.data;
          data.forEach(author => {
            this.optionsAuthors = [];
            this.optionsAuthors.push({
              id: author.id,
              name: author.name
            })
          });
        })
        .catch(() => {
          this.optionsAuthors=[];
          this.showError();
        })
    },
    async requestPostBook(book = {}) {
      await this.bookService.postBooks(book)
        .then(() => {
          this.requestGetBooks();
          this.showSuccess();
        })
        .catch(error => {
          this.$toast.add({severity:'error', summary: 'Error Register', detail:`${error.response.data.title}`, life: 3000});
        })
    },
    async requestPutBook(bookId, book = {}) {
      await this.bookService.putBooks(bookId, book)
        .then(() => {
          this.requestGetBooks();
          this.showSuccess();
        })
        .catch(() => {
          this.showError();
        })
    },
    async requestDeleteBook(bookId) {
      await this.bookService.deleteBooks(bookId)
        .then(() => {
          this.requestGetBooks();
          this.showSuccess();
        })
        .catch(() => {
          this.showError();
        })
    }
  }
}

</script>