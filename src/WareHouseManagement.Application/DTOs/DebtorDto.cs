﻿namespace WareHouseManagement.Application.DTOs;

public class DebtorDto
{
    public Guid Id { get; set; }
    public Guid? CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public string DebtorName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public decimal TotalDebt { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingDebt { get; set; }
    public DateTime DebtDate { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public string? Notes { get; set; }
    public bool IsPartnerCompany { get; set; }
    public DateTime CreatedAt { get; set; }
}

