export class User {
  constructor(
    public Id: string,
    public UserName: string,
    public NormalizedUserName: string,
    public Email: string,
    public NormalizedEmail: string,
    public EmailConfirmed: boolean,
    public PhoneNumberConfirmed: boolean,
    public PhoneNumber: string,
    public ConcurrencyStamp: string,
    public FirstName: string,
    public LastName: string,
    public Title: string,
    public EntityInfo_Created: Date,
    public EntityInfo_CreatedBy: string,
    public EntityInfo_Modified: Date,
    public EntityInfo_ModifiedBy: string,
    public EntityInfo_Deleted?: Date,
    public EntityInfo_DeletedBy?: string
  ) {}
}
