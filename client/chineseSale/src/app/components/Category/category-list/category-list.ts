import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CategoryService } from '../../../services/Category/category-service';
import { CategoryModel } from '../../../models/Category/CategoryModel';
import { CategoryCreateModel } from '../../../models/Category/CategoryCreateModel';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './category-list.html',
  styleUrls: ['./category-list.scss'],
})
export class CategoryList implements OnInit {
  categories: CategoryModel[] = [];
  showForm = false;
  editingCategory?: CategoryModel;
  newCategoryName = '';
  errorMsg = '';

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getAll().subscribe({
      next: (res) => this.categories = res,
      error: () => this.errorMsg = 'שגיאה בטעינת הקטגוריות'
    });
  }

  openAdd() {
    this.editingCategory = undefined;
    this.newCategoryName = '';
    this.showForm = true;
    this.errorMsg = '';
  }

  openEdit(cat: CategoryModel) {
    this.editingCategory = cat;
    this.newCategoryName = cat.name;
    this.showForm = true;
    this.errorMsg = '';
  }

  save() {
    if (!this.newCategoryName.trim()) return;
    const dto: CategoryCreateModel = { name: this.newCategoryName.trim() };
    this.errorMsg = '';

    if (this.editingCategory) {
      this.categoryService.update(this.editingCategory.id, dto).subscribe({
        next: () => { this.showForm = false; this.loadCategories(); },
        error: (err) => this.errorMsg = err.error?.message || 'שגיאה בעדכון'
      });
    } else {
      this.categoryService.add(dto).subscribe({
        next: () => { this.showForm = false; this.loadCategories(); },
        error: (err) => this.errorMsg = err.error?.message || 'שגיאה בהוספה'
      });
    }
  }

  cancelForm() {
    this.showForm = false;
    this.errorMsg = '';
  }

  deleteCategory(id: number) {
    if (confirm('האם אתה בטוח שברצונך למחוק את הקטגוריה?')) {
      this.categoryService.delete(id).subscribe({
        next: () => this.loadCategories(),
        error: (err) => alert(err.error?.message || 'שגיאה במחיקה')
      });
    }
  }
}
