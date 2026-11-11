var Pr=()=>{
  let[e,t]=(0,C.useState)(15);
  let n=[
    {
      id:`plan_starter`,
      name:`Starter Plan`,
      badge:`For Small Teams`,
      desc:`Essential HR automation & employee directory for growing startups.`,
      basePrice:2499,
      baseMinEmp:1,
      baseMaxEmp:10,
      perEmployeePrice:120,
      popular:!1,
      features:[
        `1-10 Employees Included in Base Slab`,
        `\u20B9120 / additional employee per month`,
        `Employee Directory & Profile Vault`,
        `Geofence & Mobile Attendance (ESS)`,
        `Leave & Holiday Approval Workflows`,
        `Self-Service Mobile App Access`
      ]
    },
    {
      id:`plan_growth`,
      name:`Growth Pro`,
      badge:`Most Popular`,
      desc:`Complete HRMS, statutory payroll calculation, and recruitment suite.`,
      basePrice:5999,
      baseMinEmp:1,
      baseMaxEmp:25,
      perEmployeePrice:150,
      popular:!0,
      features:[
        `1-25 Employees Included in Base Slab`,
        `\u20B9150 / additional employee per month`,
        `Automated Multi-State Statutory Payroll`,
        `Applicant Tracking System (ATS)`,
        `Biometric & Shift Rostering Engine`,
        `EPF, ESIC, PT, TDS Compliance Exports`,
        `Priority 24/7 Dedicated Support`
      ]
    },
    {
      id:`plan_enterprise`,
      name:`Enterprise Elite`,
      badge:`Full Suite`,
      desc:`Unlimited power with performance appraisal, training, audit readiness & custom APIs.`,
      basePrice:12999,
      baseMinEmp:1,
      baseMaxEmp:50,
      perEmployeePrice:180,
      popular:!1,
      features:[
        `1-50 Employees Included in Base Slab`,
        `\u20B9180 / additional employee per month`,
        `360\u00B0 KPI & Performance Appraisals`,
        `L&D Training LMS & Certifications`,
        `Statutory Audit & Labor Law Vault`,
        `Dedicated Enterprise Account Manager`,
        `Custom API & ERP Integrations`
      ]
    }
  ];

  let r=t=>{
    window.location.href=`/checkout/?plan=`+encodeURIComponent(t)+`&emp=`+e;
  };

  let pct=Math.round(((e-1)/149)*100);

  return(0,R.jsx)(`section`,{
    id:`pricing`,
    className:`relative w-full py-28 md:py-36 bg-transparent border-t border-white/5`,
    children:(0,R.jsxs)(`div`,{
      className:`max-w-7xl mx-auto px-6 sm:px-8 relative z-10`,
      children:[
        // Section Header
        (0,R.jsxs)(`div`,{
          className:`max-w-3xl mb-16`,
          children:[
            (0,R.jsxs)(`div`,{
              className:`inline-flex items-center gap-2 text-xs uppercase tracking-[0.25em] text-[#FBBF24] font-medium mb-4`,
              children:[
                (0,R.jsx)(`span`,{children:`Flexible SaaS Investment`}),
                (0,R.jsx)(`span`,{"aria-hidden":`true`,className:`text-[#AAA49A]/50`,children:`•`}),
                (0,R.jsx)(`span`,{className:`text-[#AAA49A]`,children:`Transparent Pricing`})
              ]
            }),
            (0,R.jsxs)(`h2`,{
              className:`text-3xl sm:text-4xl md:text-5xl font-bold tracking-tight text-[#FFFFFF] leading-[1.15] text-balance mb-6`,
              children:[
                `Transparent Plans. `,
                (0,R.jsx)(`span`,{className:`text-sora-gradient font-serif italic font-normal`,children:`Scale as You Grow.`})
              ]
            }),
            (0,R.jsx)(`p`,{
              className:`text-base sm:text-lg text-[#AAA49A] font-light leading-relaxed`,
              children:`Choose your foundational HRMS tier. Adjust your team size below to view real-time dynamic pricing with our transparent per-employee seat model.`
            })
          ]
        }),

        // Luxury Headcount Slider Box (Pure Custom HTML5 Slider - Zero Blue, Pixel-Perfect)
        (0,R.jsxs)(`div`,{
          className:`max-w-2xl mx-auto p-6 sm:p-8 rounded-xl bg-[#06060a]/90 border border-[#E8590C]/30 shadow-2xl backdrop-blur-xl mb-16 space-y-6 transition-all duration-300`,
          children:[
            (0,R.jsxs)(`div`,{
              className:`flex items-center justify-between gap-4`,
              children:[
                (0,R.jsxs)(`div`,{
                  className:`flex items-center gap-2.5`,
                  children:[
                    (0,R.jsx)(`span`,{className:`w-2.5 h-2.5 rounded-full bg-gradient-to-r from-[#E8590C] to-[#FAB005] animate-pulse`}),
                    (0,R.jsx)(`span`,{
                      className:`text-xs sm:text-sm font-semibold tracking-wider uppercase text-[#FFFFFF]`,
                      children:`Select Your Headcount:`
                    })
                  ]
                }),
                (0,R.jsxs)(`div`,{
                  className:`font-mono font-bold text-base sm:text-lg text-[#FAB005] bg-[#E8590C]/15 border border-[#FAB005]/40 px-4 py-1.5 rounded-sm shadow-sm`,
                  children:[e,` Employees`]
                })
              ]
            }),

            // Interactive Precision Track + Thumb
            (0,R.jsxs)(`div`,{
              className:`relative w-full py-4 flex items-center cursor-pointer select-none`,
              children:[
                // Background Track
                (0,R.jsxs)(`div`,{
                  className:`w-full h-2 rounded-full bg-white/10 relative overflow-visible`,
                  children:[
                    // Radiant Gold Filled Progress Bar
                    (0,R.jsx)(`div`,{
                      className:`absolute left-0 top-0 bottom-0 rounded-full bg-gradient-to-r from-[#E8590C] to-[#FAB005] shadow-[0_0_12px_rgba(250,176,5,0.5)]`,
                      style:{width:pct+`%`}
                    }),
                    // Glowing White/Gold Circular Thumb
                    (0,R.jsx)(`div`,{
                      className:`absolute top-1/2 -translate-y-1/2 -translate-x-1/2 w-6 h-6 rounded-full bg-gradient-to-tr from-[#E8590C] via-[#FAB005] to-[#FFF9DB] border-2 border-white shadow-[0_0_18px_rgba(250,176,5,0.9)] pointer-events-none transition-transform duration-75`,
                      style:{left:pct+`%`}
                    })
                  ]
                }),
                // Invisible Full-Sized Native Touch/Mouse Input Overlaid
                (0,R.jsx)(`input`,{
                  type:`range`,
                  min:`1`,
                  max:`150`,
                  value:e,
                  onChange:e=>t(parseInt(e.target.value)||1),
                  className:`absolute inset-0 w-full h-full opacity-0 cursor-pointer z-20`
                })
              ]
            }),

            (0,R.jsxs)(`div`,{
              className:`flex justify-between text-xs text-[#AAA49A] font-mono`,
              children:[
                (0,R.jsx)(`span`,{children:`1 Emp (Startup)`}),
                (0,R.jsx)(`span`,{children:`25 Emp`}),
                (0,R.jsx)(`span`,{children:`50 Emp`}),
                (0,R.jsx)(`span`,{children:`100 Emp`}),
                (0,R.jsx)(`span`,{children:`150+ Emp`})
              ]
            })
          ]
        }),

        // 3 Cards Grid (Translucent Obsidian Glass, No Badge Collision, Pristine Readability)
        (0,R.jsx)(`div`,{
          className:`grid grid-cols-1 lg:grid-cols-3 gap-8 items-stretch mb-16`,
          children:n.map(t=>{
            let a=t.popular,
                o=Math.max(0,e-t.baseMaxEmp),
                i=o*t.perEmployeePrice,
                s=t.basePrice+i;
            return(0,R.jsxs)(`div`,{
              key:t.id,
              className:`relative flex flex-col justify-between rounded-xl bg-[#07070c]/90 backdrop-blur-xl transition-all duration-300 border ${
                a
                  ? `border-[#FAB005] shadow-[0_20px_50px_rgba(232,89,12,0.3)] ring-1 ring-[#FAB005]/50 scale-[1.02] lg:-translate-y-2 z-10 pt-10 pb-8 px-8 sm:px-9`
                  : `border-white/10 hover:border-[#FAB005]/40 hover:scale-[1.01] pt-10 pb-8 px-8 sm:px-9`
              }`,
              children:[
                // Floating Badge for Popular Card (Positioned safely above top border)
                a ? (0,R.jsx)(`div`,{
                  className:`absolute -top-3.5 left-1/2 -translate-x-1/2 px-4 py-1 rounded-sm bg-gradient-to-r from-[#E8590C] via-[#FAB005] to-[#FFE066] text-[#050505] font-bold text-[10px] uppercase tracking-widest shadow-lg whitespace-nowrap`,
                  children:t.badge
                }) : null,

                (0,R.jsxs)(`div`,{
                  className:`space-y-6`,
                  children:[
                    // Card Title Area (All 3 aligned on same baseline)
                    (0,R.jsxs)(`div`,{
                      children:[
                        (0,R.jsx)(`span`,{
                          className:`text-[10px] uppercase tracking-[0.25em] font-semibold text-[#FAB005] block mb-2`,
                          children:a ? `MOST POPULAR CHOICE` : t.badge
                        }),
                        (0,R.jsx)(`h3`,{
                          className:`text-2xl sm:text-3xl font-bold tracking-tight text-[#FFFFFF]`,
                          children:t.name
                        }),
                        (0,R.jsx)(`p`,{
                          className:`text-xs text-[#AAA49A] font-light mt-2 leading-relaxed min-h-[36px]`,
                          children:t.desc
                        })
                      ]
                    }),

                    // Price Breakdown Slab Box
                    (0,R.jsxs)(`div`,{
                      className:`p-5 rounded-lg bg-[#040407]/95 border border-white/5 space-y-2.5`,
                      children:[
                        (0,R.jsxs)(`div`,{
                          className:`flex items-baseline gap-1.5`,
                          children:[
                            (0,R.jsx)(`span`,{
                              className:`text-3xl sm:text-4xl font-bold text-[#FAB005] font-serif`,
                              children:`\u20B9`+Math.round(s).toLocaleString(`en-IN`)
                            }),
                            (0,R.jsx)(`span`,{
                              className:`text-xs text-[#AAA49A] font-light`,
                              children:`/ month`
                            })
                          ]
                        }),
                        (0,R.jsxs)(`div`,{
                          className:`text-[11px] text-[#AAA49A] flex items-center justify-between font-mono pt-2 border-t border-white/5`,
                          children:[
                            (0,R.jsx)(`span`,{children:`Base (`+t.baseMinEmp+`-`+t.baseMaxEmp+` emp):`}),
                            (0,R.jsx)(`span`,{children:`\u20B9`+t.basePrice.toLocaleString(`en-IN`)})
                          ]
                        }),
                        o > 0 ? (0,R.jsxs)(`div`,{
                          className:`text-[11px] text-[#FBBF24] flex items-center justify-between font-mono`,
                          children:[
                            (0,R.jsx)(`span`,{children:`+`+o+` Extra @ \u20B9`+t.perEmployeePrice+`/emp:`}),
                            (0,R.jsx)(`span`,{children:`+\u20B9`+i.toLocaleString(`en-IN`)})
                          ]
                        }) : (0,R.jsx)(`div`,{
                          className:`text-[10px] text-emerald-400 font-medium flex items-center gap-1`,
                          children:`✓ Included in base slab (Zero extra charges)`
                        })
                      ]
                    }),

                    // Features Checklist
                    (0,R.jsx)(`ul`,{
                      className:`space-y-3 pt-2`,
                      children:t.features.map((e,t)=>(0,R.jsxs)(`li`,{
                        key:t,
                        className:`flex items-start gap-3 text-xs sm:text-[13px] text-neutral-200 font-light leading-snug`,
                        children:[
                          (0,R.jsx)(`span`,{
                            className:`text-[#FAB005] font-bold text-sm shrink-0 mt-0.5`,
                            children:`✓`
                          }),
                          (0,R.jsx)(`span`,{children:e})
                        ]
                      }))
                    })
                  ]
                }),

                // CTA Button
                (0,R.jsx)(`div`,{
                  className:`pt-8 mt-6 border-t border-white/10`,
                  children:(0,R.jsxs)(`button`,{
                    type:`button`,
                    onClick:()=>r(t.id),
                    className:a
                      ? `w-full py-3.5 px-6 rounded-sm bg-gradient-to-r from-[#E8590C] via-[#FAB005] to-[#FFE066] text-[#050505] font-semibold text-xs uppercase tracking-widest hover:shadow-[0_0_24px_rgba(232,89,12,0.5)] transition-all duration-300 flex items-center justify-center gap-2 active:scale-98 cursor-pointer`
                      : `w-full py-3.5 px-6 rounded-sm border border-[#E8590C]/40 bg-[#080808]/70 text-[#FFFFFF] hover:border-[#FAB005] hover:text-[#FAB005] hover:bg-[#E8590C]/10 font-semibold text-xs uppercase tracking-widest transition-all duration-300 flex items-center justify-center gap-2 active:scale-98 cursor-pointer`,
                    children:[
                      (0,R.jsx)(`span`,{children:`Get Started • Buy Plan`}),
                      (0,R.jsx)(`span`,{children:`→`})
                    ]
                  })
                })
              ]
            });
          })
        }),

        // Trust Badges
        (0,R.jsxs)(`div`,{
          className:`pt-8 border-t border-white/10 text-center text-xs text-[#AAA49A] flex flex-wrap items-center justify-center gap-8 sm:gap-12`,
          children:[
            (0,R.jsxs)(`span`,{
              className:`flex items-center gap-2 font-medium`,
              children:[
                (0,R.jsx)(`span`,{className:`text-emerald-400 font-bold`,children:`✓`}),
                (0,R.jsx)(`span`,{children:`Instant Workspace Activation`})
              ]
            }),
            (0,R.jsxs)(`span`,{
              className:`flex items-center gap-2 font-medium`,
              children:[
                (0,R.jsx)(`span`,{className:`text-[#FAB005] font-bold`,children:`★`}),
                (0,R.jsx)(`span`,{children:`Razorpay & UPI Verified`})
              ]
            }),
            (0,R.jsxs)(`span`,{
              className:`flex items-center gap-2 font-medium`,
              children:[
                (0,R.jsx)(`span`,{className:`text-[#FFE066] font-bold`,children:`↺`}),
                (0,R.jsx)(`span`,{children:`Cancel or Scale Seats Anytime`})
              ]
            })
          ]
        })
      ]
    })
  });
};
