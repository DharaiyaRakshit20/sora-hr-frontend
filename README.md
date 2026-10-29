# SORA | HR — Enterprise HRMS Platform

> **Theme**: Deep obsidian dark theme with warm golden-amber radial glow, inspired by the SORA HR brand identity (*"Smart HR. Stronger People."* / *"Hiring se leke retention tak — smart HR solutions jo aapke logon ko mazboot banate hain."*).

---

## 🚀 Complete Platform Architecture & Feature Specifications

### 1. Edge-to-Edge Fluid Header
- **Start (Far Left)**: Official `SORA | HR` vector logo with golden-amber separator.
- **End (Far Right)**: `Aarav Sharma` + `SUPER ADMIN` credential badge flush to the right edge (`w-full px-6 sm:px-8`).
- Cleaned: Removed duplicate "View Public Site", "Notification", and top logout button as requested.

### 2. Streamlined Sidebar Navigation
- **Dashboard Overview**: Coming Soon platform overview with quick shortcuts.
- **Admin Management**: Full-width RBAC administrators table with live search and `+ Add Admin` button.
- **Website CMS**: Collapsible submenu (Hero Banners, Blog/Articles, About Us, Privacy Policy).
- **Module Pricing Master**: Dedicated base rate configuration page where monthly fees for every HRMS module can be viewed, updated, or added dynamically.
- **Performa Quotation & Invoice**: Dynamic quotation generator with module selection, client company details, custom GST %, real-time tax calculation, high-resolution PDF download, and print support.
- **Sidebar Bottom**: Flush red-accented **Logout** button.

### 3. Two-Step Admin Creation Flow (`Admin Management` → `+ Add Admin`)
- **Step 1: Admin Profile & Identity Form**:
  - Full Name, Employee Code / ID, Work Email, Contact Phone.
  - Department (Core HR, ATS Recruitment, Payroll, Attendance & Ops, Compliance).
  - Designation Title & Organizational Role Level.
  - Click **"Next: Configure Permissions →"**.
- **Step 2: Granular Permission Matrix (View · Add · Update · Delete)**:
  - Header displays candidate profile summary and initials.
  - Table matrix containing 8 HRMS modules (Core HR, Payroll, ATS, Attendance, Compliance, Performance, Employee Portal, LMS).
  - 4 distinct action checkboxes per module: **View**, **Add**, **Update**, **Delete**.
  - Quick controls: **Row All** toggle per module, **Column All** toggles (`View All`, `Add All`, `Update All`, `Delete All`), and global **Select All / Clear All**.
  - Submitting saves the administrator into the live active table.

### 4. Module Pricing Master (`portal-pricing-master`)
- Displays all modules with current monthly base prices in INR (₹).
- Inline price editing with instant persistence into the quoting engine.
- "+ Add Custom Module" dialog for bespoke enterprise add-ons.

### 5. Performa Quotation & Invoice Generator (`portal-quotation-invoice`)
- **Configurator Panel (Left)**:
  - Client Entity Details (Company Name, GSTIN / Tax ID, State & Code, Billing Address).
  - Module Checklist with prices pulled from Module Master.
  - Headcount Tier Multipliers (1-50, 51-200, 201-500, 500+ Enterprise).
  - **Custom User-Defined GST %** input (e.g. 18%, 12%, 5%, 0%).
  - Optional Annual Discount % and custom invoice number.
- **Live Invoice Document (Right)**:
  - Corporate header with SORA HR official logo and legal credentials.
  - Client billing address, invoice number, and issue date.
  - Itemized table of selected HRMS modules with monthly and annual totals.
  - Breakdown: Gross Base Total, Discount, Taxable Subtotal, **Custom GST % Tax Amount**, and Net Grand Total.
  - Bank transfer instructions (HDFC Bank Ltd, IFSC, Account Number).
  - Authorized signatory seal and timestamp.
- **Export Capabilities**:
  - **Download Invoice PDF**: Powered by `html2pdf.js` for 1-click formatted PDF export.
  - **Print Invoice**: Optimized CSS `@media print` producing clean white paper output with dark text and zero UI clutter.

### 6. Full-Page Split Authentication
- **Left 50%**: Auto-rotating feature slider with ambient amber glow and brand quote.
- **Right 50%**:
  - **Login with OTP**: Email/Mobile input → 6-box OTP entry → auto-fill Super Admin.
  - **Login with 2FA**: Username + Master Password → TOTP 6-digit Authenticator verification.
  - **Forgot Password Flow**: Secure recovery with OTP verification and password reset.

---

## 💻 Local Preview & Usage

The application is actively running on the local development server:
> **Live Preview**: [http://localhost:5173](http://localhost:5173)

### Quick Navigation Routes:
- Public Homepage: [http://localhost:5173](http://localhost:5173)
- Full-Page Split Sign In: Click **"Sign In"** in top navbar
- Admin Dashboard: Authenticate via OTP or 2FA
- Admin Management Table: Sidebar → **Admin Management**
- 2-Step Admin Wizard: Admin Management → Click **"+ Add Admin"**
- Module Pricing Master: Sidebar → **Module Pricing Master**
- Quotation & PDF Invoice: Sidebar → **Performa Quotation & Invoice**
