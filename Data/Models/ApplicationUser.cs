using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsBlocked { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public UserInformation UserInformation { get; set; }
        public FindingPartnerUser FindingPartnerUser { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<LevelTest> LevelTests { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<PostReaction> PostReactions { get; set; }
        public ICollection<CommentReaction> CommentReactions { get; set; }
        public ICollection<Message> Messages { get; set; }

        //PartnerRequest relationship
        [InverseProperty("Sender")]
        public ICollection<PartnerRequest> SentPartnerRequests { get; set; }
        [InverseProperty("Receiver")]
        public ICollection<PartnerRequest> ReceivedPartnerRequests { get; set; }

        //Partnership relationship
        [InverseProperty("Owner")]
        public ICollection<Partnership> Partnerships { get; set; }
        [InverseProperty("Partner")]                                            //this is for creating relation in DB not necessary for query
        public ICollection<Partnership> PartnershipsAsPartner{ get; set; }

        //Conversation relationship
        [InverseProperty("Owner")]
        public ICollection<Conversation> Conversations { get; set; }
        [InverseProperty("Creator")]
        public ICollection<Conversation> ConversationsAsCreator { get; set; }

        //BlockedRelation relationship
        [InverseProperty("Owner")]
        public ICollection<BlockedRelation> BlockedRelations { get; set; }
        [InverseProperty("BlockedUser")]                                     //this is for creating relation in DB not necessary for query
        public ICollection<BlockedRelation> BlockedRelationsAsBlockedUser { get; set; }

        //Notification relationship
        [InverseProperty("Owner")]
        public ICollection<Notification> Notifications { get; set; }
        [InverseProperty("Creator")]
        public ICollection<Notification> NotificationsAsCreator { get; set; } //this is for creating relation in DB not necessary for query


    }
}
